namespace LeafBidAPI.App.Domain.Provider.Data;

/// <summary>
/// Data required to create a new provider.
/// </summary>
public record CreateProviderData(
    int UserId,
    string CompanyName
);