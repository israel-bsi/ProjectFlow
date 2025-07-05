using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.AppSettings.Request;
using ProjectFlow.Core.Models.DTOs.AppSettings.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.AppSettings;

[ApiController]
[Tags("AppSettings")]
public class UpsertAppSettingsController : ControllerBase
{
    private readonly IAppSettingsHandler _handler;

    public UpsertAppSettingsController(IAppSettingsHandler handler)
    {
        _handler = handler;
    }

    [HttpPut("v1/appsettings")]
    [EndpointSummary("Create or update App settings")]
    [Authorize]
    [ProducesResponseType(typeof(Response<AppSettingsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PutAppSettings(AppSettingsRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse(request));

        var result = await _handler.UpsertAppSettingsAsync(request);

        return result.IsSuccess
            ? Ok(result)
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}