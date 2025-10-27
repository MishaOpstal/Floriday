namespace LeafBidAPI.Application.Interfaces.Product.Resources;

/// <summary>
/// Resource representing a product.
/// </summary>
public record ProductResource(
    int Id,
    string Name,
    double Weight,
    string Picture,
    string Species,
    double? PotSize,
    double? StemLength,
    int Stock,
    int AuctionId
);