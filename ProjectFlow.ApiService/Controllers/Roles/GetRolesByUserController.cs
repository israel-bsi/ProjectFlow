using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectFlow.Core.Models.Entities;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Roles;

[ApiController]
[Tags("Role")]
public class GetRolesByUserController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public GetRolesByUserController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("v1/roles/{email}")]
    [EndpointSummary("Get roles by user")]
    [Authorize(Roles = Configuration.Roles.Admin)]
    [ProducesResponseType(typeof(Response<IList<string>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRolesByUser(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return NotFound(new ErrorData(404, description: "Usuário não encontrado"));

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(new Response<IList<string>>(roles));
    }
}