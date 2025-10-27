using System.Text.Json.Serialization;

namespace LeafBidAPI.Domain.Auctioneer.Entities;

/// <summary>
/// Represents an auctioneer in the system
/// </summary>
public class Auctioneer
{
    /// <summary>
    /// Unique identifier for the auctioneer
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Identifier of the user associated with the auctioneer
    /// </summary>
    public int UserId { get; set; }
    
    [JsonIgnore]
    public User.Entities.User User { get; set; }
}