namespace LeafBidAPI.DTOs.Page;
using LeafBidAPI.Models;

public class GetAuctionWithProductsDto
{
    public required Auction auction { get; set; }

    public required List<Product> products { get; set; }

}