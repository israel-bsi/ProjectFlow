using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Projects.Request;
using ProjectFlow.Core.Models.DTOs.Projects.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Projects;

[ApiController]
[Tags("Project")]
public class GetAllProjectsController : ControllerBase
{
    private readonly IProjectHandler _handler;

    public GetAllProjectsController(IProjectHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("v1/projects")]
    [EndpointSummary("Get all projects")]
    [Authorize]
    [ProducesResponseType(typeof(PagedResponse<List<ProjectResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetAllProjects(
        [FromQuery] int pageNumber = Core.Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Core.Configuration.DefaultPageSize,
        [FromQuery] string searchTerm = "",
        [FromQuery] string filterBy = "",
        [FromQuery] string startDate = "",
        [FromQuery] string endDate = "")
    {
        var request = new GetAllProjectsRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            FilterBy = filterBy,
            StartDate = startDate,
            EndDate = endDate
        };
        var result = await _handler.GetAllAsync(request);

        return result.IsSuccess
            ? Ok(result)
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}