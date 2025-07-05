using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Account;

[ApiController]
[Tags("Account")]
public class LogoutController : ControllerBase
{
    private readonly IAccountHandler _handler;

    public LogoutController(IAccountHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("v1/account/logout")]
    [Authorize]
    [EndpointSummary("Logout user")]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> LogoutAsync()
    {
        var result = await _handler.LogoutAsync();

        return result.IsSuccess
            ? Ok(new Response<object>(null, message: result.Message))
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}