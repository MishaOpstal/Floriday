
using LeafBidAPI.App.Domain.Auction.Entities;
using LeafBidAPI.App.Domain.Auctioneer.Entities;
using LeafBidAPI.App.Domain.AuctionSale.Entities;
using LeafBidAPI.App.Domain.AuctionSaleProduct.Entities;
using LeafBidAPI.App.Domain.Buyer.Entities;
using LeafBidAPI.App.Domain.Product.Entities;
using LeafBidAPI.App.Domain.Provider.Entities;
using LeafBidAPI.App.Domain.User.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Auction> Auctions { get; set; }
    public DbSet<Auctioneer> Auctioneers { get; set; }
    public DbSet<AuctionSale> AuctionSales { get; set; }
    public DbSet<AuctionSaleProduct> AuctionSaleProducts { get; set; }
    public DbSet<Buyer> Buyers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Provider> Providers { get; set; }
    public DbSet<User> Users { get; set; }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AuctionSale>()
            .HasOne(s => s.Auction)
            .WithMany() // of .WithMany(a => a.Sales) als je een collectie in Auction wilt
            .HasForeignKey(s => s.AuctionId)
            .OnDelete(DeleteBehavior.Restrict); // <— voorkomt cascade delete

        modelBuilder.Entity<AuctionSale>()
            .HasOne(s => s.Buyer)
            .WithMany() // idem, kan ook Buyer.Sales collectie zijn
            .HasForeignKey(s => s.BuyerId)
            .OnDelete(DeleteBehavior.Restrict); // <— voorkomt cascade delete
    }
}