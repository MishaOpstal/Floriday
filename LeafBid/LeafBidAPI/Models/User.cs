using LeafBidAPI.Enums;
using Microsoft.AspNetCore.Identity;

namespace LeafBidAPI.Models;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// Last Login
    /// </summary>
    public DateTime? LastLogin { get; set; }
}