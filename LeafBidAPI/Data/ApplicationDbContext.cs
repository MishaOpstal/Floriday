using LeafBidAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Toolbelt.ComponentModel.DataAnnotations;

namespace LeafBidAPI.Data;

// EF-Core exposes collections as DbSet<T>, which implements IQueryable<T> and thus can utilize LINQ.
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Auction> Auctions { get; set; }
    public DbSet<AuctionSales> AuctionSales { get; set; }
    public DbSet<AuctionSalesProducts> AuctionSalesProducts { get; set; }
    public DbSet<Product> Products { get; set; }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.BuildDecimalColumnTypeFromAnnotations();

        modelBuilder.Entity<AuctionSales>()
            .HasOne(s => s.Auction)
            .WithMany()
            .HasForeignKey(s => s.AuctionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AuctionSales>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Auction>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}