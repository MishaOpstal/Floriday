using LeafBidAPI.DTOs.AuctionSale;
using LeafBidAPI.Models;

namespace LeafBidAPI.Interfaces;

public interface IAuctionSaleService
{
    Task<List<AuctionSales>> GetAuctionSales();
    Task<AuctionSales> GetAuctionSaleById(int id);
    Task<AuctionSales> CreateAuctionSale(CreateAuctionSaleDto createAuctionSaleDto);
}