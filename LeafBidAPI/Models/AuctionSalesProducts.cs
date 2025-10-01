using LeafBidAPI.Enums;

namespace LeafBidAPI.Models;

public class AuctionSalesProducts
{
    public int Id { get; set; }
    public AuctionSales AuctionSale { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
}