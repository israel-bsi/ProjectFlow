using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Extension;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Projects.Request;
using ProjectFlow.Core.Models.DTOs.Projects.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Projects;

[ApiController]
[Tags("Project")]
public class UpdateProjectController : ControllerBase
{
    private readonly IProjectHandler _handler;

    public UpdateProjectController(IProjectHandler handler)
    {
        _handler = handler;
    }

    [HttpPut("v1/projects/{id:int}")]
    [EndpointSummary("Update a project")]
    [Authorize]
    [ProducesResponseType(typeof(Response<ProjectResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PutProject(ProjectRequest request, int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse(request));

        request.Id = id;
        request.User.Id = User.Id();
        var result = await _handler.UpdateAsync(request);

        return result.IsSuccess
            ? Ok(result)
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}