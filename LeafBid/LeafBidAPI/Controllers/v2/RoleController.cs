using LeafBidAPI.Interfaces;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LeafBidAPI.Controllers.v2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
[Produces("application/json")]
public class RoleController(IRoleService roleService) : ControllerBase
{
    /// <summary>
    /// Get all roles.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<IdentityRole>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<IdentityRole>>> GetRoles()
    {
        List<IdentityRole> roles = await roleService.GetRoles();
        return Ok(roles);
    }

    /// <summary>
    /// Check whether a user has a given role.
    /// </summary>
    [HttpGet("users/{userId}/has")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> GetUserHasRole(
        string userId,
        [FromQuery] string roleName)
    {
        bool hasRole = await roleService.GetUserHasRole(userId, roleName);
        return Ok(hasRole);
    }

    /// <summary>
    /// Get all users associated with a given role.
    /// </summary>
    [HttpGet("{roleName}/users")]
    [ProducesResponseType(typeof(IList<User>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<User>>> GetUsersByRole(string roleName)
    {
        IList<User> users = await roleService.GetUsersByRole(roleName);
        return Ok(users);
    }

    /// <summary>
    /// Assign roles to a user.
    /// </summary>
    [HttpPost("users/{userId}/roles")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AssignRoles(
        string userId,
        [FromBody] string[] roleNames)
    {
        bool ok = await roleService.AssignRoles(userId, roleNames);
        return ok ? NoContent() : Problem("Assign roles failed.");
    }

    /// <summary>
    /// Revoke roles from a user.
    /// </summary>
    [HttpDelete("users/{userId}/roles")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RevokeRoles(
        string userId,
        [FromBody] string[] roleNames)
    {
        bool ok = await roleService.RevokeRoles(userId, roleNames);
        return ok ? NoContent() : Problem("Revoke roles failed.");
    }
}
