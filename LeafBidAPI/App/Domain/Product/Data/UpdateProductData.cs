namespace LeafBidAPI.App.Domain.Product.Data;

/// <summary>
/// Data used to update an existing product.
/// </summary>
public record UpdateProductData(
    int Id,
    string? Name = null,
    double? Weight = null,
    string? Picture = null,
    string? Species = null,
    double? PotSize = null,
    double? StemLength = null,
    int? Stock = null,
    int? AuctionId = null
);