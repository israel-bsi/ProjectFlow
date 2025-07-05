using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Extension;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Projects.Request;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Projects;

[ApiController]
[Tags("Project")]
public class UpdateStatusProjectController : ControllerBase
{
    private readonly IProjectHandler _handler;

    public UpdateStatusProjectController(IProjectHandler handler)
    {
        _handler = handler;
    }

    [HttpPut("v1/projects/{id:int}/status")]
    [EndpointSummary("Update status of a project")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PutStatusProject(UpdateStatusProjectRequest request, int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse(request));

        request.Id = id;
        request.UserId = User.Id();
        var result = await _handler.UpdateStatusAsync(request);

        return result.IsSuccess
            ? NoContent()
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}