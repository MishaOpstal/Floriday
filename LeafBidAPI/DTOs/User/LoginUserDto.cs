namespace LeafBidAPI.DTOs.User;

public class LoginUserDto
{
    /// <summary>
    /// Data required to login a user
    /// </summary>
    
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool Remember { get; set; } = false;
}