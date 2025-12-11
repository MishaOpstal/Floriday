using System.Security.Claims;
using LeafBidAPI.Data;
using LeafBidAPI.DTOs.Auction;
using LeafBidAPI.DTOs.Product;
using LeafBidAPI.Exceptions;
using LeafBidAPI.Interfaces;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Services;

public class AuctionService(
    ApplicationDbContext context,
    UserManager<User> userManager) : IAuctionService
{
    public async Task<List<Auction>> GetAuctions()
    {
        return await context.Auctions.ToListAsync();
    }
    
    public async Task<Auction> GetAuctionById(int id)
    {
        Auction? auction = await context.Auctions.FirstOrDefaultAsync(a => a.Id == id);
        if (auction == null)
        {
            throw new NotFoundException("Auction not found");
        }

        return auction;
    }
    
    public async Task<Auction> CreateAuction(CreateAuctionDto auctionData, ClaimsPrincipal user)
    {
        User? currentUser = await userManager.GetUserAsync(user);
        if (currentUser == null)
        {
            throw new NotFoundException("User not found");
        }

        IList<string> roles = await userManager.GetRolesAsync(currentUser);
        if (!roles.Contains("Auctioneer"))
        {
            throw new UnauthorizedAccessException("User does not have the required role to create an auction");
        }

        foreach (Product product in auctionData.Products)
        {
            AuctionProducts? auctionProducts =
                await context.AuctionProducts.FirstOrDefaultAsync(a => a.ProductId == product.Id);
            if (auctionProducts != null)
            {
                throw new ProductAlreadyAssignedException("Product already assigned");
            }
        }

        Auction auction = new()
        {
            UserId = currentUser.Id,
            ClockLocationEnum = auctionData.ClockLocationEnum,
            StartDate = auctionData.StartDate
        };

        context.Auctions.Add(auction);
        await context.SaveChangesAsync();

        int counter = 1;
        foreach (Product product in auctionData.Products)
        {
            AuctionProducts auctionProduct = new()
            {
                AuctionId = auction.Id,
                ProductId = product.Id,
                ServeOrder = counter++,
                AuctionStock = product.Stock
            };

            context.AuctionProducts.Add(auctionProduct);
        }

        context.Products.UpdateRange(auctionData.Products);
        await context.SaveChangesAsync();

        return auction;
    }
    
    public async Task<Auction> UpdateAuction(int id, UpdateAuctionDto updatedAuction)
    {
        Auction? auction = await context.Auctions.FirstOrDefaultAsync(a => a.Id == id);

        if (auction == null)
        {
            throw new NotFoundException("Auction not found");
        }

        auction.StartDate = updatedAuction.StartTime;
        auction.ClockLocationEnum = updatedAuction.ClockLocationEnum;

        await context.SaveChangesAsync();
        return auction;
    }
    
    public async Task<List<Product>> GetProductsByAuctionId(int auctionId)
    {
        List<Product?> products = await context.AuctionProducts
            .Where(ap => ap.AuctionId == auctionId)
            .OrderBy(ap => ap.ServeOrder)
            .Select(ap => ap.Product)
            .ToListAsync();

        if (products == null || products.Count == 0)
        {
            throw new NotFoundException("Product not found");
        }

        return products!;
    }
}
