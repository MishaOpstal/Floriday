using LeafBidAPI.Enums;

namespace LeafBidAPI.App.Domain.User.Entities;

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
    public string Name { get; set; }
    
    /// <summary>
    /// Email of the user used for login.
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Password hash for secure authentication, don't even think about using plain text passwords Nuh uh uh!!
    /// </summary>
    public string PasswordHash { get; set; }
    public UserTypeEnum UserType { get; set; }
}