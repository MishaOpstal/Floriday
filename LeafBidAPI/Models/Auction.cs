using LeafBidAPI.Enums;

namespace LeafBidAPI.Models;

/// <summary>
/// Represents an auction
/// </summary>

public class Auction
{
    /// <summary>
    /// Unique identifier for the auction
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Description of the auction
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Start date of the auction
    /// </summary>
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// Amount of items in the auction
    /// </summary>
    public int Amount { get; set; }
    
    /// <summary>
    /// Minimum price for the auction
    /// </summary>
    public int MinimumPrice { get; set; }
    
    public ClockLocationEnum ClockLocationEnum { get; set; }
    
    public Auctioneer Auctioneer { get; set; }
}