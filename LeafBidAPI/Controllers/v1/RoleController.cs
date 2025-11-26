using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
[AllowAnonymous]
public class RoleController(ApplicationDbContext context, UserManager<User> userManager)
    : BaseController(context)
{
    /// <summary>
    /// Get all roles.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<IdentityRole>>> GetRoles()
    {
        return await Context.Roles.ToListAsync();
    }

    /// <summary>
    /// Get all users associated with a given role.
    /// </summary>
    [HttpGet("{roleName}")]
    public async Task<IList<User>> GetUsersByRole(string roleName)
    {
        return await userManager.GetUsersInRoleAsync(roleName);
    }

    /// <summary>
    /// Assign a role to a user.
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> AssignRoles(string userId, string[] roleNames)
    {
        User? user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        IdentityResult result = await userManager.AddToRolesAsync(user, roleNames);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return OkResult("Role(s) assigned successfully");
    }

    /// <summary>
    /// Revoke a role from a user.
    /// </summary>
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> RevokeRoles(string userId, string[] roleNames)
    {
        User? user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        IdentityResult result = await userManager.RemoveFromRolesAsync(user, roleNames);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return OkResult("Role(s) revoked successfully");
    }
}