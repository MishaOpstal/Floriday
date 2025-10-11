namespace LeafBidAPI.DTOs.Auction;

public class UpdateAuctionDto
{
    /// <summary>
    /// Data required to update an existing auction
    /// </summary>

    public required int Id { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public decimal? Amount { get; set; }
    public decimal? MinimumPrice { get; set; }
    public int? ClockLocationEnum { get; set; }
}