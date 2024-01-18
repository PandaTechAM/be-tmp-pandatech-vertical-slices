using PandaWebApi.DTOs.Authentication;

namespace PandaWebApi.Services.Interfaces;

public interface IAuthenticationService
{
    public Task LoginAsync(LoginDto loginDto);
    public Task LogoutAsync();
    public Task UpdatePasswordForcedAsync(string password);
    public Task UpdateOwnPassword(UpdateOwnPasswordDto updateOwnPasswordDto);
}