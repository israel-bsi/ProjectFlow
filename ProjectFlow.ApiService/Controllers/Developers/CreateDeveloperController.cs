using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Extension;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Developers.Request;
using ProjectFlow.Core.Models.DTOs.Developers.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Developers;

[ApiController]
[Tags("Developer")]
public class CreateDeveloperController : ControllerBase
{
    private readonly IDeveloperHandler _handler;

    public CreateDeveloperController(IDeveloperHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("v1/developers")]
    [EndpointSummary("Create a developer")]
    [Authorize]
    [ProducesResponseType(typeof(Response<DeveloperResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PostDeveloper(DeveloperRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse(request));

        request.User.Id = User.Id();
        var result = await _handler.CreateAsync(request);

        return result.IsSuccess
            ? CreatedAtRoute("GetDeveloperById", new { id = result.Data?.Id }, result)
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}