using LeafBidAPI.Enums;

namespace LeafBidAPI.Models;

public class Auction
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public int Amount { get; set; }
    public int MinimumPrice { get; set; }
    public ClockLocationEnum ClockLocationEnum { get; set; }
    public Auctioneer Auctioneer { get; set; }
}