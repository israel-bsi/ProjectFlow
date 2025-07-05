using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Projects.Request;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Projects;

[ApiController]
[Tags("Project")]
public class GetBudgetByProjectController : ControllerBase
{
    private readonly IProjectHandler _handler;

    public GetBudgetByProjectController(IProjectHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("v1/projects/{id:int}/budget")]
    [EndpointSummary("Get a budget by project")]
    [Authorize]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Response<ErrorData>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateBudgetAsync(int id)
    {
        var request = new GetBudgetByProjectRequest { Id = id };

        var result = await _handler.GetBudgetByProjectAsync(request);

        return result.IsSuccess
            ? File(result.Data!.FileContents, result.Data.ContentType, result.Data.FileDownloadName)
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}