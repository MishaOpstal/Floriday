namespace LeafBidAPI.DTOs.Auction;

public class CreateAuctionDto
{
    /// <summary>
    /// Data required to create a new auction
    /// </summary>

    public required decimal Amount { get; set; }
    public required int ClockLocationEnum { get; set; }
}