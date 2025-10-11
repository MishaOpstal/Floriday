namespace LeafBidAPI.DTOs.Provider;

public class UpdateProviderDto
{
    /// <summary>
    /// Data required to update a provider
    /// </summary>
    
    public required int Id { get; set; }
    public int? UserId { get; set; }
    public string? CompanyName { get; set; }
}