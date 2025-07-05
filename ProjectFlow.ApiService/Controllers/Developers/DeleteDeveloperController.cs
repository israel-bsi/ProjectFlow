using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Extension;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Developers.Request;
using ProjectFlow.Core.Models.DTOs.Developers.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Developers;

[ApiController]
[Tags("Developer")]
public class DeleteDeveloperController : ControllerBase
{
    private readonly IDeveloperHandler _handler;

    public DeleteDeveloperController(IDeveloperHandler handler)
    {
        _handler = handler;
    }

    [HttpDelete("v1/developers/{id:int}")]
    [EndpointSummary("Delete a developer")]
    [Authorize]
    [ProducesResponseType(typeof(Response<DeveloperResponse>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteDeveloper(int id)
    {
        var request = new DeleteDeveloperRequest { Id = id };

        request.User.Id = User.Id();
        var result = await _handler.DeleteAsync(request);

        return result.IsSuccess
            ? NoContent()
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}