namespace LeafBidAPI.DTOs.Auctioneer;

public class GetAuctioneerIdDto
{
    /// <summary>
    /// Data returned when requesting auctioneer information by ID
    /// </summary>
    public required int Id { get; set; }
}