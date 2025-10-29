namespace LeafBidAPI.App.Domain.AuctionSale.Data;

/// <summary>
/// Data required to create a new auction sale.
/// </summary>
public record UpdateAuctionSaleData(
    int Id,
    string PaymentReference
);