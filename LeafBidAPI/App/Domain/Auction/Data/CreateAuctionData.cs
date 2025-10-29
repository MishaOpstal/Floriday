using LeafBidAPI.Enums;

namespace LeafBidAPI.App.Domain.Auction.Data;

/// <summary>
/// Data required to create a new auction.
/// </summary>
public record CreateAuctionData(
    string Description,
    DateTime StartDate,
    int Amount,
    decimal MinimumPrice,
    ClockLocationEnum ClockLocationEnum,
    int AuctioneerId
);