namespace LeafBidAPI.DTOs.AuctionSale;

public class CreateAuctionSaleDto
{
    /// <summary>
    /// Data required to create an auction sale
    /// </summary>
    public required int AuctionId { get; set; }
    public required int BuyerId { get; set; }
    public required string PaymentReference { get; set; }
    public required DateTime Date { get; set; }
}