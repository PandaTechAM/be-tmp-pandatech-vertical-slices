using PandaWebApi.Contexts;
using PandaWebApi.Models;

namespace PandaWebApi.Services.Interfaces;

public interface ITokenService
{
    public Task<Token> CreateTokenAsync(long userId, HttpContext httpContext);
    public Task<Token> ValidateTokenAsync(string? cookie, PostgresContext dbContext);
    
    public Task UpdateTokenExpirationAsync(Token token, IConfiguration configuration, PostgresContext dbContext);
    
}