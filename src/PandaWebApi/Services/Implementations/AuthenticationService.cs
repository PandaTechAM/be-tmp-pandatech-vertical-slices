using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using PandaWebApi.Contexts;
using PandaWebApi.DTOs.Authentication;
using PandaWebApi.Enums;
using PandaWebApi.Helpers;
using PandaWebApi.Models;
using PandaWebApi.Services.Interfaces;
using ResponseCrafter.StandardHttpExceptions;

namespace PandaWebApi.Services.Implementations;

public class AuthenticationService : IAuthenticationService
{
    private readonly PostgresContext _context;
    private readonly Argon2Id _argon2Id;
    private readonly RequestContextDataProvider _requestContextDataProvider;
    private readonly string _domain;
    private readonly IConfiguration _configuration;

    public AuthenticationService(Argon2Id argon2Id, PostgresContext context, RequestContextDataProvider requestContextDataProvider, IConfiguration configuration)
    {
        _argon2Id = argon2Id;
        _context = context;
        _requestContextDataProvider = requestContextDataProvider;
        _configuration = configuration;
        _domain = configuration["Security:CookieDomain"]!;

    }
    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto, HttpContext httpContext)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username.ToLower());
        var newHistory = new UserAuthenticationHistory();
        if (user == null || user.Status == Statuses.Deleted)
        {
            throw new BadRequestException("Invalid username or password.");
        }

        if (user.Status == Statuses.Disabled)
        {
            newHistory.UserId = user.Id;
            newHistory.AuthenticationDate = DateTime.UtcNow;
            newHistory.IsAuthenticated = false;

            await _context.UserAuthenticationHistory.AddAsync(newHistory);
            await _context.SaveChangesAsync();

            throw new BadRequestException("Invalid username or password.");
        }

        var history = await _context.UserAuthenticationHistory
            .Where(u => u.UserId == user.Id)
            .OrderByDescending(u => u.AuthenticationDate)
            .Take(3).ToListAsync();

        if (history.Count == 3 && history.TrueForAll(h => !h.IsAuthenticated) &&
            history.Exists(h => h.AuthenticationDate > DateTime.UtcNow.AddSeconds(-30)))
        {
            throw new BadRequestException("Too many failed login attempts. Please try again after 30 seconds.");
        }

        if (!_argon2Id.VerifyHash(loginDto.Password, user.PasswordHash))
        {
            newHistory.UserId = user.Id;
            newHistory.AuthenticationDate = DateTime.UtcNow;
            newHistory.IsAuthenticated = false;

            await _context.UserAuthenticationHistory.AddAsync(newHistory);
            await _context.SaveChangesAsync();

            throw new BadRequestException("Invalid username or password.");
        }

        int expirationMinutesInt = 30;
        var expirationMinutesString = _configuration["Security:TokenExpirationMinutes"];
        if (Int32.TryParse(expirationMinutesString, out int expirationMinutes))
        {
            expirationMinutesInt = expirationMinutes;
        }
        
        var token = new Token
        {
            TokenString = Guid.NewGuid(),
            ExpirationDate = DateTime.UtcNow.AddMinutes(expirationMinutesInt),
            UserId = user.Id,
            CreationDate = DateTime.UtcNow
        };
        await _context.Tokens.AddAsync(token);
        await _context.SaveChangesAsync();


        var successHistory = new UserAuthenticationHistory()
        {
            UserId = user.Id,
            AuthenticationDate = DateTime.UtcNow,
            IsAuthenticated = true
        };
        await _context.UserAuthenticationHistory.AddAsync(successHistory);
        await _context.SaveChangesAsync();

        
        httpContext.Response.Cookies.Append(
            "token", token.TokenString.ToString(),
            new CookieOptions
            {
                Expires = token.ExpirationDate,
                HttpOnly = true,
                Secure = true,
                Domain = _domain
            }
        );

        var loginResponse = new LoginResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            TokenExpirationDate = token.ExpirationDate,
            Role = user.Role,
            ForcePasswordChange = user.ForcePasswordChange
        };
        return loginResponse;
    }
}