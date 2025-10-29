using LeafBidAPI.Enums;

namespace LeafBidAPI.App.Domain.Auction.Data;

/// <summary>
/// Data used to update an existing auction.
/// </summary>
public record UpdateAuctionData(
    int Id,
    string? Description = null,
    DateTime? StartDate = null,
    int? Amount = null,
    decimal? MinimumPrice = null,
    ClockLocationEnum? ClockLocationEnum = null,
    int? AuctioneerId = null
);