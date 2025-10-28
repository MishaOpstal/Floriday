namespace LeafBidAPI.App.Domain.AuctionSaleProduct.Data;

/// <summary>
/// Data required to create a new auction sale product.
/// </summary>
public record CreateAuctionSaleProductData(
    int AuctionSaleId,
    int ProductId,
    int Quantity,
    int Price
);