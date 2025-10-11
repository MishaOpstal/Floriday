namespace LeafBidAPI.DTOs.User;

public class CreateUserDto
{
    /// <summary>
    /// Data required to create a new user
    /// </summary>
    
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required int UserType { get; set; } // 0 = Buyer, 1 = Actioneer, 2 = Provider
}