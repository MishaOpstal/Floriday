using LeafBidAPI.Models;
using Microsoft.EntityFrameworkCore;
using Toolbelt.ComponentModel.DataAnnotations;

namespace LeafBidAPI.Data;

// EF-Core exposes collections as DbSet<T>, which implements IQueryable<T> and thus can utilize LINQ.
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Auction> Auctions { get; set; }
    public DbSet<Auctioneer> Auctioneers { get; set; }
    public DbSet<AuctionSales> AuctionSales { get; set; }
    public DbSet<AuctionSalesProducts> AuctionSalesProducts { get; set; }
    public DbSet<Buyer> Buyers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Provider> Providers { get; set; }
    public DbSet<User> Users { get; set; }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.BuildDecimalColumnTypeFromAnnotations();

        modelBuilder.Entity<AuctionSales>()
            .HasOne(s => s.Auction)
            .WithMany() // of .WithMany(a => a.Sales) als je een collectie in Auction wilt
            .HasForeignKey(s => s.AuctionId)
            .OnDelete(DeleteBehavior.Restrict); // <— voorkomt cascade delete

        modelBuilder.Entity<AuctionSales>()
            .HasOne(s => s.Buyer)
            .WithMany() // idem, kan ook Buyer.Sales collectie zijn
            .HasForeignKey(s => s.BuyerId)
            .OnDelete(DeleteBehavior.Restrict); // <— voorkomt cascade delete
    }
}