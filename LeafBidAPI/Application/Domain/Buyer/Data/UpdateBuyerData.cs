namespace LeafBidAPI.Domain.Buyer.Data;

/// <summary>
/// Data used to update an existing buyer.
/// </summary>
public record UpdateBuyerData(
    int Id,
    int? UserId = null,
    string? CompanyName = null
);