using LeafBidAPI.Enums;

namespace LeafBidAPI.Models;

public class AuctionSales
{
    public int Id { get; set; }
    public Auction Auction { get; set; }
    public Buyer Buyer { get; set; }
    public DateTime Date { get; set; }
    public string PaymentReference { get; set; }
}