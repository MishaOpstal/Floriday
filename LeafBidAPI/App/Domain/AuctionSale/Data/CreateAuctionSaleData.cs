namespace LeafBidAPI.App.Domain.AuctionSale.Data;

/// <summary>
/// Data required to create a new auction sale.
/// </summary>
public record CreateAuctionSaleData(
    int AuctionId,
    int BuyerId,
    DateTime Date,
    string PaymentReference
);