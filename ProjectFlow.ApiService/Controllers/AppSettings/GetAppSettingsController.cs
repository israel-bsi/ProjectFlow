using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.AppSettings.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.AppSettings;

[ApiController]
[Tags("AppSettings")]
public class GetAppSettingsController : ControllerBase
{
    private readonly IAppSettingsHandler _handler;

    public GetAppSettingsController(IAppSettingsHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("v1/appsettings")]
    [EndpointSummary("Get App settings")]
    [Authorize]
    [ProducesResponseType(typeof(Response<AppSettingsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetAppSettings()
    {
        var result = await _handler.GetAppSettingsAsync();

        return result.IsSuccess
            ? Ok(result)
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}