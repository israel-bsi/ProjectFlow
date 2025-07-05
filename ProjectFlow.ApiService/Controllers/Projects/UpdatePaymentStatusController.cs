using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Extension;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Projects.Request;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Projects;

[ApiController]
[Tags("Project")]
public class UpdatePaymentStatusController : ControllerBase
{
    private readonly IProjectHandler _handler;

    public UpdatePaymentStatusController(IProjectHandler handler)
    {
        _handler = handler;
    }

    [HttpPut("v1/projects/{id:int}/paymentstatus")]
    [EndpointSummary("Update payment status of a project")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PutStatusProject(UpdatePaymentStatusProjectRequest request, int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse(request));

        request.Id = id;
        request.UserId = User.Id();
        var result = await _handler.UpdatePaymentStatusAsync(request);

        return result.IsSuccess
            ? NoContent()
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}