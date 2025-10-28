using LeafBidAPI.Enums;

namespace LeafBidAPI.Application.Interfaces.Auction.Resources;

/// <summary>
/// Resource representing an auction.
/// </summary>
public record AuctionResource(
    int Id,
    string Description,
    DateTime StartDate,
    int Amount,
    int MinimumPrice,
    ClockLocationEnum ClockLocationEnum,
    int AuctioneerId
);