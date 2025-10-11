namespace LeafBidAPI.DTOs.Auction;

public class UpdateAuctionIdDto
{
    /// <summary>
    /// Data required to update an existing auction
    /// </summary>

    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public decimal? Amount { get; set; }
    public decimal? MinimumPrice { get; set; }
    public int? ClockLocationEnum { get; set; }
}