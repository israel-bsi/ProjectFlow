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
public class CreateProjectController : ControllerBase
{
    private readonly IProjectHandler _handler;

    public CreateProjectController(IProjectHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("v1/projects")]
    [EndpointSummary("Create a project")]
    [Authorize]
    [ProducesResponseType(typeof(Response<ProjectResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PostProject(ProjectRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse(request));

        request.User.Id = User.Id();
        var result = await _handler.CreateAsync(request);

        return result.IsSuccess
            ? CreatedAtRoute("GetProjectById", new { id = result.Data?.Id }, result)
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}