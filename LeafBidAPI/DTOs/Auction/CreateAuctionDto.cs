namespace LeafBidAPI.DTOs.Auction;

public class CreateAuctionDto
{
    /// <summary>
    /// Data required to create a new auction
    /// </summary>

    public string? Description { get; set; }
    public required DateTime StartDate { get; set; }
    public required decimal Amount { get; set; }
    public required decimal MinimumPrice { get; set; }
    public required int ClockLocationEnum { get; set; }
}