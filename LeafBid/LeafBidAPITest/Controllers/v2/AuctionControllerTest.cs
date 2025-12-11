using LeafBidAPI.Controllers.v2;
using LeafBidAPI.Exceptions;
using LeafBidAPI.Interfaces;
using LeafBidAPI.Models;
using LeafBidAPITest.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LeafBidAPITest.Controllers.v2;

public class AuctionControllerTest
{
    private readonly Mock<IAuctionService> _auctionService = new();
    private readonly Mock<IProductService> _productService = new();


    //GetAuctions
    [Fact] //success
    public async Task GetAuctions_ReturnsAllAuctions_Successfully()
    {
        // Arrange
        List<Auction> auctionList = DummyAuctions.GetFakeAuctions();
        _auctionService.Setup(s => s.GetAuctions()).ReturnsAsync(auctionList);

        AuctionController controller = new(
            _auctionService.Object,
            _productService.Object
        );

        // Act
        ActionResult<List<Auction>> result = await controller.GetAuctions();

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
        List<Auction> auctions = Assert.IsType<List<Auction>>(okResult.Value);

        Assert.Equal(4, auctions.Count);
        Assert.Equal(1, auctions[0].Id);
        Assert.Equal(2, auctions[1].Id);
        Assert.Equal(3, auctions[2].Id);
        Assert.Equal(4, auctions[3].Id);
    }
    
    //GetAuctionById
    [Fact] //fail
    public async Task GetAuctionById_ReturnsException_WhenAuctionNotFound()
    {
        // Arrange
        const int nonExistingId = 5; // ID die niet in de dummy data zit
        List<Auction> fakeAuctions = DummyAuctions.GetFakeAuctions();

        // Mock de service zodat hij een exception gooit bij een niet-bestaande ID
        _auctionService.Setup(s => s.GetAuctionById(It.Is<int>(id => fakeAuctions.All(a => a.Id != id))))
            .ThrowsAsync(new NotFoundException($"Auction with ID {nonExistingId} not found"));

        AuctionController controller = new(
            _auctionService.Object,
            _productService.Object
        );

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await controller.GetAuction(nonExistingId));
    }

    [Fact] //success
    public async Task GetAuctionById_ReturnsAuction_WhenAuctionExists()
    {
        // Arrange
        const int existingId = 2;
        List<Auction> fakeAuctions = DummyAuctions.GetFakeAuctions();
        List<Product> fakeProducts = DummyProducts.GetFakeProducts();
        
        Auction expectedAuction = fakeAuctions.First(a => a.Id == existingId);
        _auctionService.Setup(s => s.GetAuctionById(existingId))
            .ReturnsAsync(expectedAuction);
        _auctionService.Setup(s => s.GetProductsByAuctionId(2))
            .ReturnsAsync(fakeProducts);
        
        AuctionController controller = new(
            _auctionService.Object,
            _productService.Object
        );

        // Act
        ActionResult<Auction> result = await controller.GetAuction(existingId);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
        Auction auction = Assert.IsType<Auction>(okResult.Value);

        Assert.Equal(expectedAuction.Id, auction.Id);
        Assert.Equal(expectedAuction.UserId, auction.UserId);
        Assert.Equal(expectedAuction.IsLive, auction.IsLive);
    }
}