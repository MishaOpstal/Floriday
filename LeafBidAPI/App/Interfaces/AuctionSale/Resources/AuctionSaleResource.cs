namespace LeafBidAPI.App.Interfaces.AuctionSale.Resources;

/// <summary>
/// Resource representing an auction sale.
/// </summary>
public record AuctionSaleResource(
    int Id,
    int AuctionId,
    int BuyerId,
    DateTime Date,
    string PaymentReference
);