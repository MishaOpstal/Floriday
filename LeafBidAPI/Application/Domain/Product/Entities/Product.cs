using System.Text.Json.Serialization;

namespace LeafBidAPI.Domain.Product.Entities;

/// <summary>
/// Represents a product in the system.
/// </summary>
public class Product
{
    /// <summary>
    /// Unique identifier for the product.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Name of the product.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Weight of the product in kilograms.
    /// </summary>
    public double Weight { get; set; }
    
    /// <summary>
    /// Picture URL of the product.
    /// </summary>
    public string Picture { get; set; }
    
    /// <summary>
    /// Species of the product.
    /// </summary>
    public string Species { get; set; }
    
    /// <summary>
    /// Pot size of the product.
    /// </summary>
    public double? PotSize { get; set; }
    
    /// <summary>
    /// Stem length of the product.
    /// </summary>
    public double? StemLength { get; set; }
    
    /// <summary>
    /// Stock quantity of the product.
    /// </summary>
    public int Stock { get; set; }
    
    /// <summary>
    /// Auction id associated with the product.
    /// </summary>
    public required int AuctionId { get; set; }
    
    [JsonIgnore]
    public Auction.Entities.Auction? Auction { get; set; }
}