using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Account.Request;
using ProjectFlow.Core.Models.Entities;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Account;

[ApiController]
[Tags("Account")]
public class RegisterUserController : ControllerBase
{
    private readonly IAccountHandler _handler;

    public RegisterUserController(IAccountHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("v1/account/register")]
    [EndpointSummary("Register a user")]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)] 
    public async Task<ActionResult<User>> RegisterAsync([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse(request));

        var result = await _handler.RegisterAsync(request);
        return result.IsSuccess
            ? Ok(new Response<string>(null, message: result.Message))
            : this.ToActionResult((ErrorData)result.Data!);
    }
}