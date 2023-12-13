using Microsoft.AspNetCore.Mvc;
using PandaWebApi.DTOs.Authentication;
using PandaWebApi.Enums;
using PandaWebApi.Helpers;
using PandaWebApi.Services.Interfaces;

namespace PandaWebApi.Controllers;

[ApiController]
[Route("api/v1")]
[Produces("application/json")]
[Authorize(Roles.User)]
public class AuthenticationController : Controller
{
    private readonly IAuthenticationService _service;

    public AuthenticationController(IAuthenticationService service)
    {
        _service = service;
    }
    
    [UnAuthorize]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginDto loginDto)
    {
        var response = await _service.LoginAsync(loginDto, HttpContext);

        return Ok(response);
    }
}