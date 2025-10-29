namespace LeafBidAPI.App.Interfaces.Buyer.Resources;

/// <summary>
/// Resource representing a buyer.
/// </summary>
public record BuyerResource(
    int Id,
    int UserId,
    string CompanyName
);