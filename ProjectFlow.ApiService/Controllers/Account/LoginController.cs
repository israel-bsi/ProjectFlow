using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Account.Request;
using ProjectFlow.Core.Models.DTOs.Account.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Account;

[ApiController]
[Tags("Account")]
public class LoginController : ControllerBase
{
    private readonly IAccountHandler _handler;

    public LoginController(IAccountHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("v1/account/login")]
    [EndpointSummary("Authenticate user and return a token")]
    [ProducesResponseType(typeof(Response<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse(request));

        var result = await _handler.LoginAsync(request);
        return result.IsSuccess
            ? Ok(new Response<TokenResponse>(result.Data))
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}