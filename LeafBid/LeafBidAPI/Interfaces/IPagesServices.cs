using LeafBidAPI.DTOs.Page;
using LeafBidAPI.Enums;

namespace LeafBidAPI.Interfaces;

public interface IPagesServices
{
    Task<GetAuctionWithProductsDto> GetAuctionWithProducts(ClockLocationEnum clockLocation);
    Task<GetAuctionWithProductsDto> GetAuctionWithProductsById(int auctionId);
    
}