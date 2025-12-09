using LeafBidAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Toolbelt.ComponentModel.DataAnnotations;

namespace LeafBidAPI.Data;

// EF-Core exposes collections as DbSet<T>, which implements IQueryable<T> and thus can utilize LINQ.
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Auction> Auctions { get; set; }
    public DbSet<AuctionProducts> AuctionProducts { get; set; }
    public DbSet<AuctionSales> AuctionSales { get; set; }
    public DbSet<AuctionSalesProducts> AuctionSalesProducts { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.BuildDecimalColumnTypeFromAnnotations();

        // AuctionProducts (join table with payload)
        modelBuilder.Entity<AuctionProducts>()
            .HasKey(ap => new { ap.AuctionId, ap.ProductId });

        modelBuilder.Entity<AuctionProducts>()
            .HasOne(ap => ap.Auction)
            .WithMany() // or .WithMany(a => a.AuctionProducts) if you add the collection
            .HasForeignKey(ap => ap.AuctionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AuctionProducts>()
            .HasOne(ap => ap.Product)
            .WithMany() // or .WithMany(p => p.AuctionProducts)
            .HasForeignKey(ap => ap.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // AuctionSales -> Auction
        modelBuilder.Entity<AuctionSales>()
            .HasOne(s => s.Auction)
            .WithMany()
            .HasForeignKey(s => s.AuctionId)
            .OnDelete(DeleteBehavior.Restrict);

        // AuctionSales -> User
        modelBuilder.Entity<AuctionSales>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // AuctionSalesProducts -> AuctionSale
        modelBuilder.Entity<AuctionSalesProducts>()
            .HasOne(sp => sp.AuctionSale)
            .WithMany()
            .HasForeignKey(sp => sp.AuctionSaleId)
            .OnDelete(DeleteBehavior.Restrict);

        // AuctionSalesProducts -> Product
        modelBuilder.Entity<AuctionSalesProducts>()
            .HasOne(sp => sp.Product)
            .WithMany()
            .HasForeignKey(sp => sp.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Auction -> User (auctioneer)
        modelBuilder.Entity<Auction>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}