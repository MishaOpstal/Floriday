using System.Security.Claims;
using LeafBidAPI.DTOs.Auction;
using LeafBidAPI.DTOs.Product;
using LeafBidAPI.Models;

namespace LeafBidAPI.Interfaces;

public interface IAuctionService
{
    Task<List<Auction>> GetAuctions();
    Task<Auction> GetAuctionById(int id);
    Task<Auction> CreateAuction(CreateAuctionDto auctionData, ClaimsPrincipal user);
    Task<Auction> UpdateAuction(int id, UpdateAuctionDto auctionSaleData);
    Task<List<ProductResponse>> GetProductsByAuctionId(int auctionId);
}