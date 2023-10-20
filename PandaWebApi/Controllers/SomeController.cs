using Microsoft.AspNetCore.Mvc;
using PandaTech.ServiceResponse;

namespace PandaWebApi.Controllers;

[ApiController]
[Route("api/v1")]
[Produces("application/json")]
public class SomeController : ExtendedController
{
    private readonly ILogger<SomeController> _logger;

    public SomeController(IExceptionHandler exceptionHandler, ILogger<SomeController> logger) : base(
        exceptionHandler, logger)
    {
        _logger = logger;
    }

    [HttpGet("some-endpoint")]
    public ServiceResponse GetSomeEndpoint()
    {
        // _service.SomeMethod();
        return new();
    }
}