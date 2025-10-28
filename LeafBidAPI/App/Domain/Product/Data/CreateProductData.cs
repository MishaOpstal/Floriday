namespace LeafBidAPI.App.Domain.Product.Data;

/// <summary>
/// Data required to create a new product.
/// </summary>
public record CreateProductData(
    string Name,
    double Weight,
    string Picture,
    string Species,
    double? PotSize,
    double? StemLength,
    int Stock,
    int AuctionId
);