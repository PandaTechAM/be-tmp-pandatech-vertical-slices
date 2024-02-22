using BaseConverter;
using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using PandaWebApi.Contexts;
using PandaWebApi.DTOs.User;
using PandaWebApi.DTOs.UserToken;
using PandaWebApi.Enums;
using PandaWebApi.Models;
using PandaWebApi.Services.Interfaces;
using ResponseCrafter.StandardHttpExceptions;

namespace PandaWebApi.Services.Implementations;

public class UserTokenService : IUserTokenService
{
    private const int AccessTokenExpirationMinutes = 1;
    private readonly int _refreshTokenExpirationMinutes = 1440;
    private readonly int _refreshTokenMaxExpirationMinutes = int.MaxValue;
    private readonly string _domain;
    private readonly PostgresContext _context;
    private readonly HttpContext _httpContext;
    private readonly ContextUser _contextUser;

    public UserTokenService(IConfiguration configuration, PostgresContext context,
        IHttpContextAccessor httpContextAccessor, ContextUser contextUser)
    {
        _context = context;
        _contextUser = contextUser;
        _domain = configuration["Security:CookieDomain"]!;
        _httpContext = httpContextAccessor.HttpContext!;

        var refreshTokenExpirationMinutes = configuration["Security:RefreshTokenExpirationMinutes"]!;
        if (int.TryParse(refreshTokenExpirationMinutes, out var expirationMinutes))
        {
            if (expirationMinutes < 60)
            {
                _refreshTokenExpirationMinutes = 60;
            }

            _refreshTokenExpirationMinutes = expirationMinutes;
        }

        var refreshTokenMaxExpirationMinutes = configuration["Security:RefreshTokenMaxExpirationMinutes"]!;
        if (int.TryParse(refreshTokenMaxExpirationMinutes, out var maxExpirationMinutes))
        {
            _refreshTokenMaxExpirationMinutes = maxExpirationMinutes;
        }

        if (_refreshTokenExpirationMinutes > _refreshTokenMaxExpirationMinutes)
        {
            _refreshTokenExpirationMinutes = _refreshTokenMaxExpirationMinutes;
        }
    }

    public async Task<UserToken> CreateTokenAsync(IdentifyUserDto user, HttpContext httpContext)
    {
        var now = DateTime.UtcNow;

        var accessTokenSignature = Guid.NewGuid().ToString();
        var refreshTokenSignature = Guid.NewGuid().ToString();

        var token = new UserToken
        {
            UserId = user.Id,
            AccessTokenHash = Sha3.Hash(accessTokenSignature),
            RefreshTokenHash = Sha3.Hash(refreshTokenSignature),
            AccessTokenExpiresAt = now.AddMinutes(AccessTokenExpirationMinutes),
            RefreshTokenExpiresAt = now.AddMinutes(_refreshTokenExpirationMinutes),
            InitialRefreshTokenCreatedAt = now,
            CreatedAt = now
        };
        await _context.UserTokens.AddAsync(token);
        await _context.SaveChangesAsync();


        List<CookieDto> cookies =
        [
            new CookieDto("AccessToken", accessTokenSignature, token.AccessTokenExpiresAt),
            new CookieDto("RefreshToken", refreshTokenSignature, token.RefreshTokenExpiresAt),
            new CookieDto("UserId", PandaBaseConverter.Base10ToBase36(user.Id)!, token.AccessTokenExpiresAt),
            new CookieDto("Role", ((int)user.Role).ToString(), token.AccessTokenExpiresAt),
            new CookieDto("Username", user.Username, token.AccessTokenExpiresAt),
            new CookieDto("ForcePasswordChange", user.ForcePasswordChange.ToString(), token.AccessTokenExpiresAt)
        ];

        AppendCookies(cookies, httpContext, _domain);

        return token;
    }

    public async Task<IdentifyTokenDto> ValidateTokenAsync(HttpContext httpContext)
    {
        var accessTokenSignature = httpContext.Request.Cookies["AccessToken"];

        if (string.IsNullOrEmpty(accessTokenSignature))
        {
            var authHeader = httpContext.Request.Headers.Authorization.ToString();
            if (!string.IsNullOrEmpty(authHeader))
            {
                accessTokenSignature = authHeader;
            }
        }

        if (string.IsNullOrEmpty(accessTokenSignature))
        {
            throw new UnauthorizedException();
        }

        var accessTokenSignatureHash = Sha3.Hash(accessTokenSignature);

        var token = await _context.UserTokens
            .Include(u => u.User)
            .FirstOrDefaultAsync(x => x.AccessTokenHash == accessTokenSignatureHash);

        if (token is null || token.AccessTokenExpiresAt < DateTime.UtcNow || token.User.Status != Statuses.Active)
            throw new UnauthorizedException();

        var response = new IdentifyTokenDto
        {
            TokenId = token.Id,
            AccessTokenSignature = accessTokenSignature,
            CreatedAt = token.CreatedAt,
            ExpirationDate = token.AccessTokenExpiresAt,
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

    public async Task RevokeTokenAsync()
    {
        var now = DateTime.UtcNow;
        var tokenId = _context.UserTokens.FirstOrDefault(x => x.Id == _contextUser.TokenId);
        var token = await _context.UserTokens.FirstOrDefaultAsync(x => x.Id == tokenId!.Id);
        if (token is null)
        {
            throw new BadRequestException("invalid_token");
        }

        token.AccessTokenExpiresAt = now;
        token.RefreshTokenExpiresAt = now;
        await _context.SaveChangesAsync();
    }

    public async Task RevokeAllTokensAsync(long userId)
    {
        var now = DateTime.UtcNow;
        var tokens = await _context.UserTokens
            .Where(x => x.UserId == userId
                        && (x.AccessTokenExpiresAt > now || x.RefreshTokenExpiresAt > now))
            .ToListAsync();

        foreach (var token in tokens)
        {
            if (token.AccessTokenExpiresAt > now)
                token.AccessTokenExpiresAt = now;
            if (token.RefreshTokenExpiresAt > now)
                token.RefreshTokenExpiresAt = now;
        }

        await _context.SaveChangesAsync();
    }

    public async Task RevokeAllTokensExceptCurrentAsync()
    {
        var now = DateTime.UtcNow;
        var tokens = await _context.UserTokens
            .Where(x => x.UserId == _contextUser.Id
                        && x.Id != _contextUser.TokenId
                        && (x.AccessTokenExpiresAt > now || x.RefreshTokenExpiresAt > now))
            .ToListAsync();

        foreach (var token in tokens)
        {
            if (token.AccessTokenExpiresAt > now)
                token.AccessTokenExpiresAt = now;
            if (token.RefreshTokenExpiresAt > now)
                token.RefreshTokenExpiresAt = now;
        }

        await _context.SaveChangesAsync();
    }

    public async Task RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
    {
        var now = DateTime.UtcNow;
        var refreshTokenHash = Sha3.Hash(refreshTokenDto.RefreshToken);

        var userToken = await _context.UserTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.RefreshTokenHash == refreshTokenHash);

        if (userToken is null || userToken.RefreshTokenExpiresAt <= now)
        {
            throw new BadRequestException("invalid_refresh_token");
        }

        var newExpirationDate = now.AddMinutes(_refreshTokenExpirationMinutes);

        if (newExpirationDate > userToken.InitialRefreshTokenCreatedAt.AddMinutes(_refreshTokenMaxExpirationMinutes))
        {
            newExpirationDate = userToken.InitialRefreshTokenCreatedAt.AddMinutes(_refreshTokenMaxExpirationMinutes);
        }

        if (newExpirationDate <= now.AddMinutes(60))
        {
            newExpirationDate = now.AddMinutes(60);
        }

        var newAccessTokenSignature = Guid.NewGuid().ToString();
        var newRefreshTokenSignature = Guid.NewGuid().ToString();

        var newToken = new UserToken
        {
            UserId = userToken.UserId,
            PreviousUserTokenId = userToken.Id,
            AccessTokenHash = Sha3.Hash(newAccessTokenSignature),
            RefreshTokenHash = Sha3.Hash(newRefreshTokenSignature),
            AccessTokenExpiresAt = now.AddMinutes(AccessTokenExpirationMinutes),
            RefreshTokenExpiresAt = newExpirationDate,
            InitialRefreshTokenCreatedAt = userToken.InitialRefreshTokenCreatedAt,
            CreatedAt = now
        };

        await _context.UserTokens.AddAsync(newToken);
        await _context.SaveChangesAsync();

        List<CookieDto> cookies =
        [
            new CookieDto("AccessToken", newAccessTokenSignature, newToken.AccessTokenExpiresAt),
            new CookieDto("RefreshToken", newRefreshTokenSignature, newToken.RefreshTokenExpiresAt),
            new CookieDto("UserId", PandaBaseConverter.Base10ToBase36(userToken.User.Id)!,
                newToken.AccessTokenExpiresAt),
            new CookieDto("Role", ((int)userToken.User.Role).ToString(), newToken.AccessTokenExpiresAt),
            new CookieDto("Username", userToken.User.Username, newToken.AccessTokenExpiresAt),
            new CookieDto("ForcePasswordChange", userToken.User.ForcePasswordChange.ToString(),
                newToken.AccessTokenExpiresAt)
        ];
        AppendCookies(cookies, _httpContext, _domain);
    }

    private static void AppendCookies(List<CookieDto> cookies, HttpContext httpContext, string domain)
    {
        foreach (var cookie in cookies)
        {
            httpContext.Response.Cookies.Append(
                cookie.Key, cookie.Value,
                new CookieOptions
                {
                    Expires = cookie.ExpirationDate,
                    HttpOnly = true,
                    Secure = true,
                    Domain = domain
                }
            );
        }
    }
}