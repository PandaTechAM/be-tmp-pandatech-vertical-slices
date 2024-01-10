using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using PandaWebApi.Contexts;
using PandaWebApi.DTOs;
using PandaWebApi.DTOs.Authentication;
using PandaWebApi.DTOs.Token;
using PandaWebApi.DTOs.User;
using PandaWebApi.Enums;
using PandaWebApi.Models;
using PandaWebApi.Services.Interfaces;
using ResponseCrafter.StandardHttpExceptions;

namespace PandaWebApi.Services.Implementations;

[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
public class AuthenticationService : IAuthenticationService
{
    private readonly PostgresContext _context;
    private readonly Argon2Id _argon2Id;
    private readonly ContextUser _contextUser;
    private readonly ITokenService _tokenService;
    private readonly HttpContext _httpContext;

    public AuthenticationService(Argon2Id argon2Id, PostgresContext context,
        ContextUser contextUser, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _argon2Id = argon2Id;
        _contextUser = contextUser;
        _tokenService = tokenService;
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public async Task LoginAsync(LoginDto loginDto)
    {
        var isValidPassword = Password.Validate(loginDto.Password, 8, true, true, true, false);
        
        if (!isValidPassword)
            throw new BadRequestException(
                "password_should_contain_at_least_8_characters_one_lowercase_one_uppercase_one_digit");


        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username.ToLower());
        var newHistory = new UserAuthenticationHistory();
        if (user == null || user.Status == Statuses.Deleted)
        {
            throw new BadRequestException("invalid_username_or_password");
        }

        if (user.Status == Statuses.Disabled)
        {
            newHistory.UserId = user.Id;
            newHistory.CreatedAt = DateTime.UtcNow;
            newHistory.IsAuthenticated = false;

            await _context.UserAuthenticationHistory.AddAsync(newHistory);
            await _context.SaveChangesAsync();

            throw new BadRequestException("invalid_username_or_password");
        }

        var history = await _context.UserAuthenticationHistory
            .Where(u => u.UserId == user.Id)
            .OrderByDescending(u => u.CreatedAt)
            .Take(3).ToListAsync();

        if (history.Count == 3 && history.TrueForAll(h => !h.IsAuthenticated) &&
            history.Exists(h => h.CreatedAt > DateTime.UtcNow.AddSeconds(-30)))
        {
            throw new BadRequestException("too_many_failed_attempts.try_again_later");
        }

        if (!_argon2Id.VerifyHash(loginDto.Password, user.PasswordHash))
        {
            newHistory.UserId = user.Id;
            newHistory.CreatedAt = DateTime.UtcNow;
            newHistory.IsAuthenticated = false;

            await _context.UserAuthenticationHistory.AddAsync(newHistory);
            await _context.SaveChangesAsync();

            throw new BadRequestException("invalid_username_or_password");
        }


        var successHistory = new UserAuthenticationHistory
        {
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            IsAuthenticated = true
        };
        await _context.UserAuthenticationHistory.AddAsync(successHistory);
        await _context.SaveChangesAsync();


        var identifiedUser = new IdentifyUserDto
        {
            Id = user.Id,
            Role = user.Role,
            ForcePasswordChange = user.ForcePasswordChange,
            Username = user.Username
        };
        await _tokenService.CreateTokenAsync(identifiedUser, _httpContext);
    }

    public async Task LogoutAsync()
    {
        var tokenId = _contextUser.TokenId;
        var token = await _context.Tokens.FirstOrDefaultAsync(t => t.Id == tokenId);
        token!.ExpirationDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        foreach (var cookie in _httpContext.Request.Cookies)
        {
            _httpContext.Response.Cookies.Delete(cookie.Key);
        }
    }

    public async Task UpdatePasswordForced(string password)
    {
        var tokenId = _contextUser.TokenId;

        var tokenToVerify = await _context.Tokens.Include(u => u.User)
            .FirstOrDefaultAsync(t => t.Id == tokenId);

        if (tokenToVerify == null || tokenToVerify.ExpirationDate < DateTime.UtcNow)
            throw new UnauthorizedException();

        if (tokenToVerify.User.Status != Statuses.Active)
            throw new UnauthorizedException();

        if (!tokenToVerify.User.ForcePasswordChange)
            throw new BadRequestException("cannot_use_this_action_as_password_change_is_not_forced");

        var user = tokenToVerify.User;

        user.PasswordHash = _argon2Id.HashPassword(password);
        user.ForcePasswordChange = false;
        await _context.SaveChangesAsync();

        await LogoutAllExceptCurrentSessionAsync(user.Id, tokenId);
    }

    public async Task UpdateOwnPassword(UpdateOwnPasswordDto updateOwnPasswordDto)
    {
        var isValidPassword = Password.Validate(updateOwnPasswordDto.NewPassword, 8, true, true, true, false);

        if (!isValidPassword)
            throw new BadRequestException(
                "password_should_contain_at_least_8_characters_one_lowercase_one_uppercase_one_digit");

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == _contextUser.TokenId);

        if (user == null)
            throw new NotFoundException();

        if (user.Role == Roles.SuperAdmin)
            throw new ForbiddenException("superadmin_password_cannot_be_changed");

        if (!_argon2Id.VerifyHash(updateOwnPasswordDto.OldPassword, user.PasswordHash))
            throw new BadRequestException("wrong_old_password");

        user.PasswordHash = _argon2Id.HashPassword(updateOwnPasswordDto.NewPassword);
        await _context.SaveChangesAsync();

        await LogoutAllExceptCurrentSessionAsync(user.Id, _contextUser.Id);
    }

    public async Task LogoutAllAsync(long userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            throw new NotFoundException();

        var tokens = await _context.Tokens.Where(t => t.UserId == userId).ToListAsync();

        if (tokens.Count == 0)
            return;

        foreach (var userToken in tokens.Where(t => t.ExpirationDate > DateTime.UtcNow))
        {
            userToken.ExpirationDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    private async Task LogoutAllExceptCurrentSessionAsync(long userId, long tokenId)
    {
        var tokens = await _context.Tokens
            .Where(t => t.UserId == userId && t.Id != tokenId).ToListAsync();

        foreach (var expiredUserToken in tokens.Where(t => t.ExpirationDate > DateTime.UtcNow))
        {
            expiredUserToken.ExpirationDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }
}