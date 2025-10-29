namespace LeafBidAPI.App.Domain.AuctionSaleProduct.Data;

/// <summary>
/// Data used to update an existing auction sale product.
/// </summary>
public record UpdateAuctionSaleProductData(
    int Id,
    int? AuctionSaleId = null,
    int? ProductId = null,
    int? Quantity = null,
    decimal? Price = null
);
