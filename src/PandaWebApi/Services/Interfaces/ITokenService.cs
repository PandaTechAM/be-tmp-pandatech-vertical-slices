using PandaWebApi.Contexts;
using PandaWebApi.DTOs.Authentication;
using PandaWebApi.DTOs.Token;
using PandaWebApi.Models;

namespace PandaWebApi.Services.Interfaces;

public interface ITokenService
{
    public Task<Token> CreateTokenAsync(IdentifyUserDto user, HttpContext httpContext);
    public Task<IdentifyTokenDto> ValidateTokenAsync(PostgresContext dbContext, HttpContext httpContext);
    
    public Task UpdateTokenExpirationAsync(IdentifyTokenDto token, IConfiguration configuration, PostgresContext dbContext, HttpContext httpContext);
    
}