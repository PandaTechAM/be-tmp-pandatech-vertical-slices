﻿using Microsoft.AspNetCore.Mvc;
using PandaWebApi.Attributes;
using PandaWebApi.DTOs.Authentication;
using PandaWebApi.Enums;
using PandaWebApi.Services.Interfaces;

namespace PandaWebApi.Controllers;

[ApiController]
[Route("api/v1")]
[Produces("application/json")]
[Authorize(Roles.User)]
public class AuthenticationController(IAuthenticationService service) : Controller
{
    [Anonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
    {
        var response = await service.LoginAsync(loginDto);

        return Ok(response);
    }

    [Authorize(Roles.User)]
    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        await service.LogoutAsync();
        return Ok();
    }

    [Authorize(Roles.User)]
    [HttpGet("user-identify")]
    public IActionResult IdentifyUser()
    {
        var data = service.IdentifyUser();

        return Ok(data);
    }

    [Authorize(Roles.User)]
    [HttpPatch("change-own-password")]
    public async Task<IActionResult> ChangeOwnPassword([FromBody] UpdateOwnPasswordDto updatePasswordDto)
    {
        await service.UpdateOwnPassword(updatePasswordDto);
        return Ok();
    }

    [AllowPasswordChange]
    [HttpPatch("change-own-password-forced")]
    public async Task<IActionResult> ChangeOwnPasswordForced([FromBody] UpdatePasswordForced password)
    {
        await service.UpdatePasswordForced(password.Password);
        return Ok();
    }
}