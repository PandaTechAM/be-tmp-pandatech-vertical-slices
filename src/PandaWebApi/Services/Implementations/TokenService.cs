using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using PandaWebApi.Contexts;
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

    public async Task<Token> CreateTokenAsync(long userId, HttpContext httpContext)
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
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Tokens.AddAsync(token);
        await _context.SaveChangesAsync();

        httpContext.Response.Cookies.Append(
            "Token", tokenSignature,
            new CookieOptions
            {
                Expires = token.ExpirationDate,
                HttpOnly = true,
                Secure = true,
                Domain = _domain
            }
        );
        return token;
    }

    public async Task<Token> ValidateTokenAsync(string? cookie, PostgresContext dbContext)
    {
        if (string.IsNullOrEmpty(cookie))
            throw new UnauthorizedException();

        var tokenSignature = new Guid(cookie).ToString();
        var tokenSignatureHash = Sha3.Hash(tokenSignature);
        var token = await dbContext.Tokens.Include(z => z.User)
            .FirstOrDefaultAsync(x => x.SignatureHash == tokenSignatureHash);

        if (token == null || token.ExpirationDate < DateTime.UtcNow || token.User.Status != Statuses.Active)
            throw new UnauthorizedException();

        return token;
    }

    public async Task UpdateTokenExpirationAsync(Token token, IConfiguration configuration, PostgresContext dbContext)
    {
        var expirationMinutesInt = 15;
        if (Int32.TryParse(configuration["Security:TokenExpirationMinutes"], out int expirationMinutes))
        {
            expirationMinutesInt = expirationMinutes;
        }

        var maxExpirationMinutesInt = 360;
        if (Int32.TryParse(configuration["Security:TokenMaxExpirationMinutes"], out int maxExpirationMinutes))
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
    }
}