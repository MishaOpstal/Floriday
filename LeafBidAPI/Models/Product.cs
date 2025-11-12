using System.Text.Json.Serialization;

namespace LeafBidAPI.Models;

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
    /// Description of the product.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Minimum Price of the product.
    /// </summary>
    public string MinPrice { get; set; }

    /// <summary>
    /// Max Price of the product.
    /// </summary>
    public string MaxPrice { get; set; }

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
    /// Region of the product.
    /// </summary>
    public string Region { get; set; }

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
    /// Harvested date and time of the product.
    /// </summary>
    public DateTime? HarvestedAt { get; set; }

    /// <summary>
    /// Provider id associated with the product.
    /// </summary>
    public required int ProviderId { get; set; }

    [JsonIgnore]
    public Provider? Provider { get; set; }
    
    /// <summary>
    /// Auction id associated with the product.
    /// </summary>
    public required int AuctionId { get; set; }
    
    [JsonIgnore]
    public Auction? Auction { get; set; }
}