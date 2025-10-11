namespace LeafBidAPI.DTOs.Auctioneer;

public class CreateAuctioneerDto
{
    /// <summary>
    /// Data required to create a new auctioneer
    /// </summary>
    
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required int UserType { get; set; }
}
