namespace LeafBidAPI.Models;

/// <summary>
/// Represents an auctioneer in the system
/// </summary>
public class Auctioneer
{
    /// <summary>
    /// Unique identifier for the auctioneer
    /// </summary>
    public int Id { get; set; }
    public User User { get; set; }
}