using LeafBidAPI.Data;
using LeafBidAPI.DTOs.AuctionSale;
using LeafBidAPI.Exceptions;
using LeafBidAPI.Interfaces;
using LeafBidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Services;

public class AuctionSaleService(ApplicationDbContext context) : IAuctionSaleService
{

    public async Task<List<AuctionSales>> GetAuctionSales()
    {
        return await context.AuctionSales.ToListAsync();
    }

    public async Task<AuctionSales> GetAuctionSaleById(int id)
    {
        AuctionSales? auctionSale = await context.AuctionSales.Where(sale => sale.Id == id).FirstOrDefaultAsync();
        if (auctionSale == null)
        {
            throw new NotFoundException("Auction sale not found");
        }

        return auctionSale;
    }

    public async Task<AuctionSales> CreateAuctionSale(CreateAuctionSaleDto auctionSaleData)
    {
        AuctionSales auctionSale = new()
        {
            AuctionId = auctionSaleData.AuctionId,
            UserId = auctionSaleData.UserId,
            PaymentReference = auctionSaleData.PaymentReference,
            Date = auctionSaleData.Date
        };
        
        // Create the auction sale
        context.AuctionSales.Add(auctionSale);
        await context.SaveChangesAsync();

        return auctionSale;
    }
}