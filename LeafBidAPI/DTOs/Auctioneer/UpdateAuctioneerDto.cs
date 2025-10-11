namespace LeafBidAPI.DTOs.Auctioneer;

public class UpdateAuctioneerDto
{
    /// <summary>
    /// Data required to update an auctioneer
    /// </summary>
    public required int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int? UserType { get; set; }
}