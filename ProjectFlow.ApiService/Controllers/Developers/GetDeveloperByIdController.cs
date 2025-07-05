using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Developers.Request;
using ProjectFlow.Core.Models.DTOs.Developers.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Developers;

[ApiController]
[Tags("Developer")]
public class GetDeveloperByIdController : ControllerBase
{
    private readonly IDeveloperHandler _handler;

    public GetDeveloperByIdController(IDeveloperHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("v1/developers/{id:int}", Name = "GetDeveloperById")]
    [EndpointSummary("Get developer by id")]
    [Authorize]
    [ProducesResponseType(typeof(Response<DeveloperResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Response<ErrorData>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response<ErrorData>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetDeveloperById(int id)
    {
        var request = new GetDeveloperByIdRequest { Id = id };

        var result = await _handler.GetByIdAsync(request);
        
        return result.IsSuccess
            ? Ok(result)
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}