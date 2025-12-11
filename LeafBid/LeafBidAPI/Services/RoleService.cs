using LeafBidAPI.Data;
using LeafBidAPI.Exceptions;
using LeafBidAPI.Interfaces;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Services;

public class RoleService(
    ApplicationDbContext context,
    UserManager<User> userManager) : IRoleService
{
    /// <summary>
    /// Get all roles.
    /// </summary>
    /// <returns></returns>
    public async Task<List<IdentityRole>> GetRoles()
    {
        return await context.Roles.ToListAsync();
    }

    /// <summary>
    /// Check whether a user has a given role.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<bool> GetUserHasRole(string userId, string roleName)
    {
        User? user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
        
        IList<string> roles = await userManager.GetRolesAsync(user);
        return roles.Contains(roleName);
    }

    /// <summary>
    /// Get all users associated with a given role.
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public async Task<IList<User>> GetUsersByRole(string roleName)
    {
        return await userManager.GetUsersInRoleAsync(roleName);
    }

    /// <summary>
    /// Assign roles to a user.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleNames"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<bool> AssignRoles(string userId, string[] roleNames)
    {
        User? user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
        
        IdentityResult result = await userManager.AddToRolesAsync(user, roleNames);
        return result.Succeeded;
    }

    /// <summary>
    /// Revoke roles from a user.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleNames"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<bool> RevokeRoles(string userId, string[] roleNames)
    {
        User? user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
        
        IdentityResult result = await userManager.RemoveFromRolesAsync(user, roleNames);
        return result.Succeeded;
    }
}