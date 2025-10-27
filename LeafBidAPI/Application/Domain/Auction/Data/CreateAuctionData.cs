using LeafBidAPI.Enums;

namespace LeafBidAPI.Domain.Auction.Data;

/// <summary>
/// Data required to create a new auction.
/// </summary>
public record CreateAuctionData(
    string Description,
    DateTime StartDate,
    int Amount,
    int MinimumPrice,
    ClockLocationEnum ClockLocationEnum,
    int AuctioneerId
);