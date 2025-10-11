namespace LeafBidAPI.DTOs.Provider;

public class CreateProviderDto
{
    /// <summary>
    /// Data required to create a provider
    /// </summary>
    
    public required int UserId { get; set; }
    public required string CompanyName { get; set; }
}