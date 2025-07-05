using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Developers.Request;
using ProjectFlow.Core.Models.DTOs.Developers.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Developers;

[ApiController]
[Tags("Developer")]
public class UpdateDeveloperController : ControllerBase
{
    private readonly IDeveloperHandler _handler;

    public UpdateDeveloperController(IDeveloperHandler handler)
    {
        _handler = handler;
    }

    [HttpPut("v1/developers/{id:int}")]
    [EndpointSummary("Update a developer")]
    [Authorize]
    [ProducesResponseType(typeof(Response<DeveloperResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PutDeveloper(DeveloperRequest request, int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse(request));

        request.Id = id;
        var result = await _handler.UpdateAsync(request);

        return result.IsSuccess
            ? Ok(result)
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}