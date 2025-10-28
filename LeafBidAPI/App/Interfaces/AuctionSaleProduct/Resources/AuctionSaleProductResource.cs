namespace LeafBidAPI.Application.Interfaces.AuctionSaleProduct.Resources;

/// <summary>
/// Resource representing an auction sale product entry.
/// </summary>
public record AuctionSaleProductResource(
    int Id,
    int AuctionSaleId,
    int ProductId,
    int Quantity,
    int Price
);