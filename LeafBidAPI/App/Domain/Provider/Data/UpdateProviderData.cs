namespace LeafBidAPI.App.Domain.Provider.Data;

/// <summary>
/// Data used to update an existing provider.
/// </summary>
public record UpdateProviderData(
    int Id,
    int? UserId = null,
    string? CompanyName = null
);