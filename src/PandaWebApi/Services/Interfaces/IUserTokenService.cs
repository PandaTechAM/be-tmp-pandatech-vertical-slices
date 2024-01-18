using PandaWebApi.Contexts;
using PandaWebApi.DTOs.UserToken;
using PandaWebApi.Models;

namespace PandaWebApi.Services.Interfaces;

public interface IUserTokenService
{
    public Task<UserToken> CreateTokenAsync(IdentifyUserDto user, HttpContext httpContext);
    public Task<IdentifyTokenDto> ValidateTokenAsync(HttpContext httpContext);
    public Task RevokeTokenAsync();
    public Task RevokeAllTokensAsync(long userId);
    public Task RevokeAllTokensExceptCurrentAsync();
    public Task RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    
}