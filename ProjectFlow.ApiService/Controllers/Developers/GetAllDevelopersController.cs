using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Developers.Request;
using ProjectFlow.Core.Models.DTOs.Developers.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Developers;

[ApiController]
[Tags("Developer")]
public class GetAllDevelopersController : ControllerBase
{
    private readonly IDeveloperHandler _handler;

    public GetAllDevelopersController(IDeveloperHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("v1/developers")]
    [EndpointSummary("Get all developers")]
    [Authorize]
    [ProducesResponseType(typeof(PagedResponse<List<DeveloperResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetAllDevelopers(
        [FromQuery] int pageNumber = Core.Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Core.Configuration.DefaultPageSize,
        [FromQuery] string searchTerm = "")
    {
        var request = new GetAllDevelopersRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm
        };
        var result = await _handler.GetAllAsync(request);

        return result.IsSuccess
            ? Ok(result)
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}