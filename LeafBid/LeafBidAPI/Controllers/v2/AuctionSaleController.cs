using LeafBidAPI.Data;
using LeafBidAPI.DTOs.AuctionSale;
using LeafBidAPI.Interfaces;
using LeafBidAPI.Models;
using LeafBidAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
[Authorize]
// [AllowAnonymous]
public class AuctionSaleController(IAuctionSaleService auctionSaleService) : ControllerBase()
{
    /// <summary>
    /// Get all auction sales
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<AuctionSales>>> GetAuctionSales()
    {
        return await auctionSaleService.GetAuctionSales();
    }

    /// <summary>
    /// Get auction sale by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuctionSales>> GetAuctionSaleById(int id)
    {
        return await auctionSaleService.GetAuctionSaleById(id);
    }

    /// <summary>
    /// Create a new auction sale
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AuctionSales>> CreateAuctionSale([FromBody] CreateAuctionSaleDto auctionSaleData)
    {
        return await auctionSaleService.CreateAuctionSale(auctionSaleData);
    }
}