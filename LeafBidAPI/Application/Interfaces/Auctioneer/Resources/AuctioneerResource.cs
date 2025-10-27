namespace LeafBidAPI.Application.Interfaces.Auctioneer.Resources;

/// <summary>
/// Resource representing an auctioneer.
/// </summary>
public record AuctioneerResource(
    int Id,
    int UserId
);