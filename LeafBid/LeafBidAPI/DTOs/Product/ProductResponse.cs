using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace LeafBidAPI.DTOs.Product;

public class ProductResponse
{
    public required int Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string Description { get; set; }

    [Decimal(10,2)]
    public required decimal MinPrice { get; set; }

    [Decimal(10,2)]
    public decimal? MaxPrice { get; set; }

    public required double Weight { get; set; }

    public string? Picture { get; set; }

    public required string Species { get; set; }

    public required string Region { get; set; }

    public double? PotSize { get; set; }

    public double? StemLength { get; set; }

    public required int Stock { get; set; }

    public required DateTime HarvestedAt { get; set; }

    public required string ProviderUserName { get; set; }
}