namespace LeafBidAPI.DTOs.Page;
using Models;

public class GetAuctionWithProductsDto
{
    public required Auction Auction { get; set; }

    public required List<Product> Products { get; set; }
}