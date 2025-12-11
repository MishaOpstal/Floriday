using LeafBidAPI.Data;
using LeafBidAPI.DTOs.Page;
using LeafBidAPI.DTOs.Product;
using LeafBidAPI.Enums;
using LeafBidAPI.Interfaces;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v2;

/// <summary>
/// get auction and product from the input ID
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
[Authorize]
public class PagesController(IPagesServices pagesServices) : ControllerBase
{
    /// <summary>
    /// Get the closest auction and its products for a given clock location
    /// </summary>
    [HttpGet("closest/{clockLocationEnum}")]
    public async Task<ActionResult<GetAuctionWithProductsDto>> GetAuctionWithProducts(
        ClockLocationEnum clockLocationEnum)
    {
        GetAuctionWithProductsDto auction = await pagesServices.GetAuctionWithProducts(clockLocationEnum);
        return Ok(auction);
    }

    /// <summary>
    /// Get the auction and provided products using the auction id
    /// </summary>
    [HttpGet("{auctionId:int}")]
    public async Task<ActionResult<GetAuctionWithProductsDto>> GetAuctionWithProductsById(int auctionId)
    {
        GetAuctionWithProductsDto result = await pagesServices.GetAuctionWithProductsById(auctionId);
        return Ok(result);
    }
}