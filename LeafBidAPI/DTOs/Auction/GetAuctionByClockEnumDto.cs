namespace LeafBidAPI.DTOs.Auction;
using LeafBidAPI.Models;

public class GetAuctionByClockEnumDto
{
    public Auction auction { get; set; }

    public List<Product> product { get; set; }
}