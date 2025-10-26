namespace LeafBidAPI.DTOs.Auctioneer;

public class UpdateAuctioneerDto
{
    /// <summary>
    /// Data required to update an auctioneer
    /// </summary>
    public required int Id { get; set; }
    public int? UserId { get; set; }
}