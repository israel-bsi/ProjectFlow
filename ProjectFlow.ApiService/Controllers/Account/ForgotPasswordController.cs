using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Account.Request;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Account;

[ApiController]
[Tags("Account")]
public class ForgotPasswordController : ControllerBase
{
    private readonly IAccountHandler _handler;

    public ForgotPasswordController(IAccountHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("v1/account/forgot-password")]
    [EndpointSummary("Send a reset password link to user email")]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse(request));

        var result = await _handler.ForgotPasswordAsync(request);
        return result.IsSuccess
            ? Ok(new Response<object>(null, message: result.Message))
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}