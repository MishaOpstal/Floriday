namespace LeafBidAPI.Domain.Buyer.Data;

/// <summary>
/// Data required to create a new buyer.
/// </summary>
public record CreateBuyerData(
    int UserId,
    string CompanyName
);
