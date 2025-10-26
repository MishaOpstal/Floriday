namespace LeafBidAPI.DTOs.User;

public class UpdateUserDto
{
    /// <summary>
    /// Data required to update a user
    /// </summary>
    
    public required int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int? UserType { get; set; } // 0 = Buyer, 1 = Actioneer, 2 = Provider
}