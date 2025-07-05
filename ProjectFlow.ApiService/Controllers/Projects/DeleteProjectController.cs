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
public class DeleteProjectController : ControllerBase
{
    private readonly IProjectHandler _handler;

    public DeleteProjectController(IProjectHandler handler)
    {
        _handler = handler;
    }

    [HttpDelete("v1/projects/{id:int}")]
    [EndpointSummary("Delete a project")]
    [Authorize]
    [ProducesResponseType(typeof(Response<ProjectResponse>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteProject(int id)
    {
        var request = new DeleteProjectRequest { Id = id };

        request.User.Id = User.Id();
        var result = await _handler.DeleteAsync(request);

        return result.IsSuccess
            ? NoContent()
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}