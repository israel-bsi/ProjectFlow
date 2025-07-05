using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Account.Response;
using ProjectFlow.Core.Models.Entities;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Account;

[ApiController]
[Tags("Account")]
public class GetUserInfoController : ControllerBase
{
    private readonly IAccountHandler _handler;

    public GetUserInfoController(IAccountHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("v1/account/{id:int}")]
    [Authorize]
    [EndpointSummary("Get user info")]
    [ProducesResponseType(typeof(Response<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<User>> GetUserInfo([FromRoute] int id)
    {
        var result = await _handler.GetUserInfoAsync(id);

        return result.IsSuccess
            ? Ok(result.Data)
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}