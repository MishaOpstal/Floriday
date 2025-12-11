using LeafBidAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace LeafBidAPI.Interfaces;

public interface IRoleService
{
    Task<List<IdentityRole>> GetRoles();
    Task<bool> GetUserHasRole(string userId, string roleName);
    Task<IList<User>> GetUsersByRole(string roleName);
    Task<bool> AssignRoles(string userId, string[] roleNames);
    Task<bool> RevokeRoles(string userId, string[] roleNames);
}