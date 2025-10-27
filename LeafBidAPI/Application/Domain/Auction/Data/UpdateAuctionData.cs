using LeafBidAPI.Enums;

namespace LeafBidAPI.Domain.Auction.Data;

/// <summary>
/// Data used to update an existing auction.
/// </summary>
public record UpdateAuctionData(
    int Id,
    string? Description = null,
    DateTime? StartDate = null,
    int? Amount = null,
    int? MinimumPrice = null,
    ClockLocationEnum? ClockLocationEnum = null,
    int? AuctioneerId = null
);