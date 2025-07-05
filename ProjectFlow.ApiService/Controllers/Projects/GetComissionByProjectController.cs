using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Projects.Request;
using ProjectFlow.Core.Models.DTOs.Projects.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Projects;

[ApiController]
[Tags("Project")]
public class GetComissionByProjectController : ControllerBase
{
    private readonly IProjectHandler _handler;

    public GetComissionByProjectController(IProjectHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("v1/projects/{id:int}/commission")]
    [EndpointSummary("Get commission by project")]
    [Authorize]
    [ProducesResponseType(typeof(Response<ProjectCommissionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetCommissions(int id)
    {
        var request = new GetProjectByIdRequest { Id = id };

        var result = await _handler.GetCommissionsAsync(request);

        return result.IsSuccess
            ? Ok(result)
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}