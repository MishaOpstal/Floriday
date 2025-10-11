namespace LeafBidAPI.DTOs.Buyer;

public class UpdateBuyerDto
{
    /// <summary>
    /// Data required to update a buyer
    /// </summary>
    
    public required int Id { get; set; }
    public int? UserId { get; set; }
    public string? CompanyName { get; set; }
}