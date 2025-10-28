namespace LeafBidAPI.App.Domain.Auctioneer.Data;

/// <summary>
/// Data required to create a new auctioneer.
/// </summary>
public record CreateAuctioneerData(
    int UserId
);