using LeafBidAPI.Data;
using LeafBidAPI.DTOs.Page;
using LeafBidAPI.DTOs.Product;
using LeafBidAPI.Enums;
using LeafBidAPI.Exceptions;
using LeafBidAPI.Interfaces;
using LeafBidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Services;

public class PagesServices(ApplicationDbContext context) : IPagesServices
{
    public async Task<GetAuctionWithProductsDto> GetAuctionWithProducts(ClockLocationEnum clockLocation)
    {
        Auction? auction = await context.Auctions
            .Where(a => a.ClockLocationEnum == clockLocation)
            .OrderBy(a => a.StartDate)
            .FirstOrDefaultAsync();

        if (auction == null)
        {
            throw new NotFoundException("Auction not found.");
        }

        List<Product?> products = await context.AuctionProducts
            .Where(ap => ap.AuctionId == auction.Id)
            .OrderBy(ap => ap.ServeOrder)
            .Select(ap => ap.Product)
            .Where(p => p != null)
            .ToListAsync();

        if (products.Count == 0)
        {
            throw new NotFoundException("No products found for this auction.");
        }

        ProductService productService = new(context);
        List<ProductResponse> productResponse = products
            .OfType<Product>()
            .Select(product => productService.CreateProductResponse(product))
            .ToList();

        GetAuctionWithProductsDto result = new()
        {
            Auction = auction,
            Products = productResponse
        };

        return result;
    }
    
    public async Task<GetAuctionWithProductsDto> GetAuctionWithProductsById(int auctionId)
    {
        Auction? auction = await context.Auctions
            .Where(a => a.Id == auctionId)
            .FirstOrDefaultAsync();

        if (auction == null)
        {
            throw new NotFoundException("Auction not found.");
        }

        List<Product?> products = await context.AuctionProducts
            .Where(ap => ap.AuctionId == auction.Id)
            .OrderBy(ap => ap.ServeOrder)
            .Select(ap => ap.Product)
            .Where(p => p != null)
            .ToListAsync();

        if (products.Count == 0)
        {
            throw new NotFoundException("No products found for this auction.");
        }

        ProductService productService = new(context);
        List<ProductResponse> productResponse = products
            .OfType<Product>()
            .Select(product => productService.CreateProductResponse(product))
            .ToList();

        GetAuctionWithProductsDto result = new()
        {
            Auction = auction,
            Products = productResponse
        };

        return result;
    }
}
