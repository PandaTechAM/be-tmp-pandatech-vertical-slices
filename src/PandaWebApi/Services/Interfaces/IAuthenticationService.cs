using PandaWebApi.DTOs.Authentication;

namespace PandaWebApi.Services.Interfaces;

public interface IAuthenticationService
{
    public Task<IdentifyUserDto> LoginAsync(LoginDto loginDto);
    public Task LogoutAsync();
    public IdentifyUserDto IdentifyUser();
    public Task UpdatePasswordForced(string password);
    public Task UpdateOwnPassword(UpdateOwnPasswordDto updateOwnPasswordDto);
    public Task LogoutAllAsync(long userId);
}