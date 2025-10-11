namespace LeafBidAPI.DTOs.Buyer;

public class CreateBuyerDto
{
    /// <summary>
    /// Data required to create a buyer
    ///  </summary>
    
    public required int UserId { get; set; }
    public required string CompanyName { get; set; }
}