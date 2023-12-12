using PandaWebApi.DTOs.Authentication;

namespace PandaWebApi.Services.Interfaces;

public interface IAuthenticationService
{
    public Task<LoginResponseDto> LoginAsync(LoginDto loginDto, HttpContext httpContext);
}