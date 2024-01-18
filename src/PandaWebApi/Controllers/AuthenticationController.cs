using Microsoft.AspNetCore.Mvc;
using PandaWebApi.Attributes;
using PandaWebApi.DTOs.Authentication;
using PandaWebApi.DTOs.UserToken;
using PandaWebApi.Enums;
using PandaWebApi.Services.Interfaces;

namespace PandaWebApi.Controllers;

[ApiController]
[Route("api/v1/authentication")]
[Produces("application/json")]
[Authorize(Roles.User)]
public class AuthenticationController(IAuthenticationService authService, IUserTokenService userTokenService) : Controller
{
    [Anonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
    {
        await authService.LoginAsync(loginDto);

        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        await authService.LogoutAsync();
        return Ok();
    }
    [Anonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
    {
        await userTokenService.RefreshTokenAsync(refreshTokenDto);
        return Ok();
    }

    [HttpPatch("change-own-password")]
    public async Task<IActionResult> ChangeOwnPassword([FromBody] UpdateOwnPasswordDto updatePasswordDto)
    {
        await authService.UpdateOwnPassword(updatePasswordDto);
        return Ok();
    }

    [AllowPasswordChange]
    [HttpPatch("change-own-password-forced")]
    public async Task<IActionResult> ChangeOwnPasswordForced([FromBody] UpdatePasswordForced password)
    {
        await authService.UpdatePasswordForcedAsync(password.Password);
        return Ok();
    }
}