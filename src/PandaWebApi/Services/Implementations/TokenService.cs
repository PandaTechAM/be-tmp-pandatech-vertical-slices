using System.Diagnostics.CodeAnalysis;
using BaseConverter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Pandatech.Crypto;
using PandaWebApi.Contexts;
using PandaWebApi.DTOs.Authentication;
using PandaWebApi.DTOs.Token;
using PandaWebApi.Enums;
using PandaWebApi.Models;
using PandaWebApi.Services.Interfaces;
using ResponseCrafter.StandardHttpExceptions;

namespace PandaWebApi.Services.Implementations;

[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
public class TokenService : ITokenService
{
    private readonly string _tokenExpirationMinutes;
    private readonly string _tokenMaxExpirationMinutes;
    private readonly string _domain;
    private readonly PostgresContext _context;

    public TokenService(IConfiguration configuration, PostgresContext context)
    {
        _context = context;
        _tokenExpirationMinutes = configuration["Security:TokenExpirationMinutes"]!;
        _tokenMaxExpirationMinutes = configuration["Security:TokenMaxExpirationMinutes"]!;
        _domain = configuration["Security:CookieDomain"]!;
    }

    public async Task<Token> CreateTokenAsync(IdentifyUserDto user, HttpContext httpContext)
    {
        var expirationMinutesInt = 15;

        if (int.TryParse(_tokenExpirationMinutes, out var expirationMinutes))
        {
            expirationMinutesInt = expirationMinutes;
        }

        var maxExpirationMinutesInt = 360;

        if (int.TryParse(_tokenMaxExpirationMinutes, out var maxExpirationMinutes))
        {
            maxExpirationMinutesInt = maxExpirationMinutes;
        }

        if (expirationMinutesInt > maxExpirationMinutesInt)
        {
            expirationMinutesInt = maxExpirationMinutesInt;
        }

        var tokenSignature = Guid.NewGuid().ToString();

        var token = new Token
        {
            SignatureHash = Sha3.Hash(tokenSignature),
            ExpirationDate = DateTime.UtcNow.AddMinutes(expirationMinutesInt),
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Tokens.AddAsync(token);
        await _context.SaveChangesAsync();
        
        var cookies = new Dictionary<string, string>
        {
            { "Token", tokenSignature },
            { "UserId", PandaBaseConverter.Base10ToBase36(user.Id)! },
            { "Role", ((int)user.Role).ToString() },
            { "Username", user.Username },
            { "ForcePasswordChange", user.ForcePasswordChange.ToString() }
        };

        AppendCookies(cookies, httpContext, _domain, token.ExpirationDate);

        return token;
    }

    public async Task<IdentifyTokenDto> ValidateTokenAsync(PostgresContext dbContext, HttpContext httpContext)
    {
        var tokenSignature = httpContext.Request.Cookies["Token"];

        if (string.IsNullOrEmpty(tokenSignature))
            throw new UnauthorizedException();

        var tokenSignatureHash = Sha3.Hash(tokenSignature);
        var token = await dbContext.Tokens.Include(z => z.User)
            .FirstOrDefaultAsync(x => x.SignatureHash == tokenSignatureHash);

        if (token == null || token.ExpirationDate < DateTime.UtcNow || token.User.Status != Statuses.Active)
            throw new UnauthorizedException();

        var response = new IdentifyTokenDto
        {
            TokenId = token.Id,
            TokenSignature = tokenSignature,
            CreatedAt = token.CreatedAt,
            ExpirationDate = token.ExpirationDate,
            User = new IdentifyUserDto
            {
                Id = token.User.Id,
                Role = token.User.Role,
                ForcePasswordChange = token.User.ForcePasswordChange,
                Username = token.User.Username
            }
        };
        
        return response;
    }

    public async Task UpdateTokenExpirationAsync(IdentifyTokenDto token, IConfiguration configuration,
        PostgresContext dbContext,
        HttpContext httpContext)
    {
        var expirationMinutesInt = 15;
        if (int.TryParse(configuration["Security:TokenExpirationMinutes"], out var expirationMinutes))
        {
            expirationMinutesInt = expirationMinutes;
        }

        var maxExpirationMinutesInt = 360;
        if (int.TryParse(configuration["Security:TokenMaxExpirationMinutes"], out var maxExpirationMinutes))
        {
            maxExpirationMinutesInt = maxExpirationMinutes;
        }

        var newExpirationDate = DateTime.UtcNow.AddMinutes(expirationMinutesInt);

        if (newExpirationDate < token.CreatedAt.AddMinutes(maxExpirationMinutesInt))
        {
            token.ExpirationDate = newExpirationDate;
        }
        else
        {
            token.ExpirationDate = token.CreatedAt.AddMinutes(maxExpirationMinutesInt);
        }

        await dbContext.SaveChangesAsync();
        var cookies = new Dictionary<string, string>
        {
            { "Token", token.TokenSignature },
            { "UserId", PandaBaseConverter.Base10ToBase36(token.User.Id)! },
            { "Role", ((int)token.User.Role).ToString() },
            { "Username", token.User.Username },
            { "ForcePasswordChange", token.User.ForcePasswordChange.ToString() }
        };
        AppendCookies(cookies, httpContext, _domain, token.ExpirationDate);
    }

    private static void AppendCookies(Dictionary<string, string> cookies, HttpContext httpContext, string domain,
        DateTime expirationDate)
    {
        foreach (var cookie in cookies)
        {
            httpContext.Response.Cookies.Append(
                cookie.Key, cookie.Value,
                new CookieOptions
                {
                    Expires = expirationDate,
                    HttpOnly = true,
                    Secure = true,
                    Domain = domain
                }
            );
        }
    }
}