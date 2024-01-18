using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using PandaWebApi.Contexts;
using PandaWebApi.DTOs.Authentication;
using PandaWebApi.DTOs.User;
using PandaWebApi.DTOs.UserToken;
using PandaWebApi.Enums;
using PandaWebApi.Models;
using PandaWebApi.Services.Interfaces;
using ResponseCrafter.StandardHttpExceptions;

namespace PandaWebApi.Services.Implementations;

public class AuthenticationService(
    Argon2Id argon2Id,
    PostgresContext context,
    ContextUser contextUser,
    IUserTokenService userTokenService,
    IHttpContextAccessor httpContextAccessor)
    : IAuthenticationService
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext!;

    public async Task LoginAsync(LoginDto loginDto)
    {
        var isValidPassword = Password.Validate(loginDto.Password, 8, true, true, true, false);

        if (!isValidPassword)
            throw new BadRequestException(
                "password_should_contain_at_least_8_characters_one_lowercase_one_uppercase_one_digit");

        var normalizedUsername = loginDto.Username.ToLower();
        
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == normalizedUsername);
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

            await context.UserAuthenticationHistory.AddAsync(newHistory);
            await context.SaveChangesAsync();

            throw new BadRequestException("invalid_username_or_password");
        }

        var history = await context.UserAuthenticationHistory
            .Where(u => u.UserId == user.Id)
            .OrderByDescending(u => u.CreatedAt)
            .Take(3).ToListAsync();

        if (history.Count == 3 && history.TrueForAll(h => !h.IsAuthenticated) &&
            history.Exists(h => h.CreatedAt > DateTime.UtcNow.AddSeconds(-30)))
        {
            throw new BadRequestException("too_many_failed_attempts.try_again_later");
        }

        if (!argon2Id.VerifyHash(loginDto.Password, user.PasswordHash))
        {
            newHistory.UserId = user.Id;
            newHistory.CreatedAt = DateTime.UtcNow;
            newHistory.IsAuthenticated = false;

            await context.UserAuthenticationHistory.AddAsync(newHistory);
            await context.SaveChangesAsync();

            throw new BadRequestException("invalid_username_or_password");
        }


        var successHistory = new UserAuthenticationHistory
        {
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            IsAuthenticated = true
        };
        await context.UserAuthenticationHistory.AddAsync(successHistory);
        await context.SaveChangesAsync();


        var identifiedUser = new IdentifyUserDto
        {
            Id = user.Id,
            Role = user.Role,
            ForcePasswordChange = user.ForcePasswordChange,
            Username = user.Username
        };
        await userTokenService.CreateTokenAsync(identifiedUser, _httpContext);
    }

    public async Task LogoutAsync()
    {
        await userTokenService.RevokeTokenAsync();

        foreach (var cookie in _httpContext.Request.Cookies)
        {
            _httpContext.Response.Cookies.Delete(cookie.Key);
        }
    }

    public async Task UpdatePasswordForcedAsync(string password)
    {
        var tokenId = contextUser.TokenId;

        var tokenToVerify = await context.UserTokens.Include(u => u.User)
            .FirstOrDefaultAsync(t => t.Id == tokenId);

        if (tokenToVerify == null || tokenToVerify.AccessTokenExpiresAt < DateTime.UtcNow)
            throw new UnauthorizedException();

        if (tokenToVerify.User.Status != Statuses.Active)
            throw new UnauthorizedException();

        if (!tokenToVerify.User.ForcePasswordChange)
            throw new BadRequestException("cannot_use_this_action_as_password_change_is_not_forced");

        var user = tokenToVerify.User;

        user.PasswordHash = argon2Id.HashPassword(password);
        user.ForcePasswordChange = false;
        await context.SaveChangesAsync();

        await userTokenService.RevokeAllTokensExceptCurrentAsync();
    }

    public async Task UpdateOwnPassword(UpdateOwnPasswordDto updateOwnPasswordDto)
    {
        var isValidPassword = Password.Validate(updateOwnPasswordDto.NewPassword, 8, true, true, true, false);

        if (!isValidPassword)
            throw new BadRequestException(
                "password_should_contain_at_least_8_characters_one_lowercase_one_uppercase_one_digit");

        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == contextUser.TokenId);

        if (user == null)
            throw new NotFoundException();

        if (user.Role == Roles.SuperAdmin)
            throw new ForbiddenException("superadmin_password_cannot_be_changed");

        if (!argon2Id.VerifyHash(updateOwnPasswordDto.OldPassword, user.PasswordHash))
            throw new BadRequestException("wrong_old_password");

        user.PasswordHash = argon2Id.HashPassword(updateOwnPasswordDto.NewPassword);
        await context.SaveChangesAsync();

        await userTokenService.RevokeAllTokensExceptCurrentAsync();
    }
}