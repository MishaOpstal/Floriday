using LeafBidAPI.Enums;

namespace LeafBidAPI.Models;

public class AuctionSales
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public required Auction Auction { get; set; }
    public int BuyerId { get; set; }
    public required Buyer Buyer { get; set; }
    public DateTime Date { get; set; }
    public required string PaymentReference { get; set; }
}