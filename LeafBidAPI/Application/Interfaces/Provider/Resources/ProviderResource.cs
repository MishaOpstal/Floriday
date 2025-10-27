namespace LeafBidAPI.Application.Interfaces.Provider.Resources;

/// <summary>
/// Resource representing a provider.
/// </summary>
public record ProviderResource(
    int Id,
    int UserId,
    string CompanyName
);