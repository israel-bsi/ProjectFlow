using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Models.DTOs.Account.Request;
using ProjectFlow.Core.Models.Entities;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Roles;

[ApiController]
[Tags("Role")]
public class RemoveRoleFromUserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public RemoveRoleFromUserController(UserManager<User> userManager,
        RoleManager<IdentityRole<int>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpDelete("v1/roles")]
    [Authorize(Roles = Configuration.Roles.Admin)]
    [EndpointSummary("Remove role from a user")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveRoleFromUser([FromBody] RoleRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse(request));

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return NotFound(new ErrorData(404, description: "Usuário não encontrado"));

        var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
        if (!roleExists)
            return BadRequest(new ErrorData(description: $"A role '{request.RoleName}' não existe."));

        var result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);

        if (result.Succeeded)
            return NoContent();

        var errors = result.Errors.Select(e => new Error
        {
            Field = e.Code,
            Message = e.Description
        });
        var errorData = new ErrorData(400, "Erro ao remover role ao usuário", errors.ToList());
        return BadRequest(errorData);
    }
}