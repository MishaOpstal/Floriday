using System.ComponentModel.DataAnnotations;
using LeafBidAPI.App.Domain.User.Enums;

namespace LeafBidAPI.App.Domain.User.Models;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User
{
    /// <summary>
    /// Unique identifier for the user.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// full name of the user.
    /// </summary>
    [MaxLength(255)]
    public required string Name { get; set; }
    
    /// <summary>
    /// Email of the user used for login.
    /// </summary>
    [MaxLength(255)]
    public required string Email { get; set; }
    
    /// <summary>
    /// Password hash for secure authentication, don't even think about using plain text passwords Nuh uh uh!!
    /// </summary>
    [MaxLength(255)]
    public required string PasswordHash { get; set; }
    public UserTypeEnum UserType { get; set; }
}