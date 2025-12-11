using LeafBidAPI.Data;
using LeafBidAPI.DTOs.AuctionSaleProduct;
using LeafBidAPI.Exceptions;
using LeafBidAPI.Interfaces;
using LeafBidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Services;

public class AuctionSaleProductService(ApplicationDbContext context) : IAuctionSaleProductService
{

    public async Task<List<AuctionSalesProducts>> GetAuctionSaleProducts()
    {
        return await context.AuctionSalesProducts.ToListAsync();
    }

    public async Task<AuctionSalesProducts> GetAuctionSaleProductById(int id)
    {
        var auctionSaleProduct = await context.AuctionSalesProducts.Where(asp => asp.Id == id).FirstOrDefaultAsync();
        if (auctionSaleProduct == null)
        {
            throw new NotFoundException("Auction sale product not found");
        }

        return auctionSaleProduct;
    }

    public async Task<AuctionSalesProducts> CreateAuctionSaleProduct(CreateAuctionSaleProductDto auctionSaleProductData)
    {
        AuctionSalesProducts auctionSaleProduct = new()
        {
            AuctionSaleId = auctionSaleProductData.AuctionSaleId,
            ProductId = auctionSaleProductData.ProductId,
            Quantity = auctionSaleProductData.Quantity,
            Price = auctionSaleProductData.Price
        };
        context.AuctionSalesProducts.Add(auctionSaleProduct);
        await context.SaveChangesAsync();
        return auctionSaleProduct;
    }

    public async Task<AuctionSalesProducts> UpdateAuctionSaleProduct(int id, UpdateAuctionSaleProductDto auctionSaleProductData)
    {
        
        AuctionSalesProducts? auctionSaleProducts =  await context.AuctionSalesProducts.Where(asp => asp.Id == id).FirstOrDefaultAsync();
        
        if (auctionSaleProducts == null)
        {
            throw new NotFoundException("Auction sale product not found");
        }

        auctionSaleProducts.Id = auctionSaleProductData.Id;
        auctionSaleProducts.AuctionSaleId = auctionSaleProductData.AuctionSaleId;
        auctionSaleProducts.ProductId = auctionSaleProductData.ProductId;
        auctionSaleProducts.Quantity = auctionSaleProductData.Quantity;
        auctionSaleProducts.Price = auctionSaleProductData.Price;
        

        await context.SaveChangesAsync();
        return auctionSaleProducts;
    }
}