using LeafBidAPI.DTOs.AuctionSaleProduct;
using LeafBidAPI.Models;

namespace LeafBidAPI.Interfaces;

public interface IAuctionSaleProductService
{
    Task<List<AuctionSalesProducts>> GetAuctionSaleProducts();
    Task<AuctionSalesProducts> GetAuctionSaleProductById(int id);
    Task<AuctionSalesProducts> CreateAuctionSaleProduct(CreateAuctionSaleProductDto auctionSaleProductData);
    Task<AuctionSalesProducts> UpdateAuctionSaleProduct(int id, UpdateAuctionSaleProductDto auctionSaleProductData);
}