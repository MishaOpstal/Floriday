using LeafBidAPI.Data;
using LeafBidAPI.DTOs.Page;
using LeafBidAPI.DTOs.Product;
using LeafBidAPI.Enums;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

/// <summary>
/// get auction and product from the input ID
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class PagesController(ApplicationDbContext dbContext) : BaseController(dbContext)
{
    /// <summary>
    /// Get the closest auction and its products for a given clock location
    /// </summary>
    [HttpGet("closest/{clockLocationEnum}")]
    public async Task<ActionResult<GetAuctionWithProductsDto>> GetAuctionWithProducts(
        ClockLocationEnum clockLocationEnum)
    {
        Auction? auction = await Context.Auctions
            .Where(a => a.ClockLocationEnum == clockLocationEnum)
            .OrderBy(a => a.StartDate)
            .FirstOrDefaultAsync();

        if (auction == null)
        {
            return NotFound("Auction not found.");
        }

        List<Product?> products = await Context.AuctionProducts
            .Where(ap => ap.AuctionId == auction.Id)
            .OrderBy(ap => ap.ServeOrder)
            .Select(ap => ap.Product)          // requires navigation property AuctionProducts.Product
            .Where(p => p != null)
            .ToListAsync();

        if (products.Count == 0)
        {
            return NotFound("No products found for this auction.");
        }
        
        ProductController productController = new(Context);
        List<ProductResponse> productResponse = products.OfType<Product>()
            .Select(product => productController.CreateProductResponse(product))
            .ToList();

        GetAuctionWithProductsDto result = new()
        {
            Auction = auction,
            Products = productResponse
        };

        return Ok(result);
    }

    /// <summary>
    /// Get the auction and provided products using the auction id
    /// </summary>
    [HttpGet("{auctionId:int}")]
    public async Task<ActionResult<GetAuctionWithProductsDto>> GetAuctionWithProducts(int auctionId)
    {
        Auction? auction = await Context.Auctions
            .Where(a => a.Id == auctionId)
            .FirstOrDefaultAsync();

        if (auction == null)
        {
            return NotFound("Auction not found.");
        }

        List<Product?> products = await Context.AuctionProducts
            .Where(ap => ap.AuctionId == auction.Id)
            .OrderBy(ap => ap.ServeOrder)
            .Select(ap => ap.Product) // requires navigation property AuctionProducts.Product
            .Where(p => p != null)
            .ToListAsync();

        if (products.Count == 0)
        {
            return NotFound("No products found for this auction.");
        }

        ProductController productController = new(Context);
        List<ProductResponse> productResponse = products.OfType<Product>()
            .Select(product => productController.CreateProductResponse(product))
            .ToList();
        
        GetAuctionWithProductsDto result = new()
        {
            Auction = auction,
            Products = productResponse
        };

        return Ok(result);
    }
}