namespace LeafBidAPI.DTOs.Auction;

public class UpdateAuctionDto
{
    /// <summary>
    /// Data required to update an existing auction
    /// </summary>

    public required int Id { get; set; }
    public int? ClockLocationEnum { get; set; }
}