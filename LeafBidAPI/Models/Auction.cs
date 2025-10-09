using System.Text.Json.Serialization;
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
    
    /// <summary>
    /// Identifier of the auctioneer associated with the auction
    /// </summary>
    public required int AuctioneerId { get; set; }
    
    [JsonIgnore]
    public Auctioneer? Auctioneer { get; set; }
}