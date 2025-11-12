namespace LeafBidAPI.DTOs.Page;
using LeafBidAPI.Models;

public class GetPageDto
{
    public required Auction auction { get; set; }

    public required Product product { get; set; }

}