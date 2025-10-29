using LeafBidAPI.App.Domain.Auction.Enums;

namespace LeafBidAPI.App.Interfaces.Auction.Resources;

/// <summary>
/// Resource representing an auction.
/// </summary>
public record AuctionResource(
    int Id,
    string Description,
    DateTime StartDate,
    int Amount,
    decimal MinimumPrice,
    ClockLocationEnum ClockLocationEnum,
    int AuctioneerId
);