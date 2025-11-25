using LeafBidAPI.Enums;
using Microsoft.AspNetCore.Identity;

namespace LeafBidAPI.Models;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// full name of the user.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Email of the user used for login.
    /// </summary>
    public string Email { get; set; }
}