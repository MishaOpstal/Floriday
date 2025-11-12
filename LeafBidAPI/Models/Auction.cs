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
    /// Start date of the auction
    /// </summary>
    public DateTime StartDate { get; set; }
    
    public ClockLocationEnum ClockLocationEnum { get; set; }
    
    /// <summary>
    /// Identifier of the auctioneer associated with the auction
    /// </summary>
    public required int AuctioneerId { get; set; }
    
    [JsonIgnore]
    public Auctioneer? Auctioneer { get; set; }
}