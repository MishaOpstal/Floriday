namespace LeafBidAPI.App.Domain.Auctioneer.Data;

/// <summary>
/// Data used to update an existing auctioneer.
/// </summary>
public record UpdateAuctioneerData(
    int Id,
    int? UserId = null
);