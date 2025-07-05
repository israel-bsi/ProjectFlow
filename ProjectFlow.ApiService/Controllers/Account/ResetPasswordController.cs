using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Account.Request;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Account;

[ApiController]
[Tags("Account")]
public class ResetPasswordController : ControllerBase
{
    private readonly IAccountHandler _handler;

    public ResetPasswordController(IAccountHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("v1/account/reset-password")]
    [EndpointSummary("Reset user password")]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ResetPassword(
        [FromQuery] string userId,
        [FromQuery] string token,
        [FromBody] ResetPasswordRequest request)
    {
        request.UserId = userId;
        request.Token = token;

        var result = await _handler.ResetPasswordAsync(request);
        return result.IsSuccess
            ? Ok(new Response<string>(null, message: result.Message))
            : this.ToActionResult((ErrorData)result.Data!);
    }
}