using LeafBidAPI.Enums;
using LeafBidAPI.Models;
using Microsoft.EntityFrameworkCore;

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
        
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Provider)
            .WithMany() // idem, kan ook Buyer.Sales collectie zijn
            .HasForeignKey(p => p.ProviderId)
            .OnDelete(DeleteBehavior.Restrict); // <— voorkomt cascade delete

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1, Name = "Nick", Email = "test@email.com", PasswordHash = "hashed_password_1",
                UserType = UserTypeEnum.Auctioneer
            },
            new User
            {
                Id = 2, Name = "Robin", Email = "test@email.com", PasswordHash = "hashed_password_2",
                UserType = UserTypeEnum.Buyer
            },
            new User
            {
                Id = 3, Name = "Misha", Email = "test@email.com", PasswordHash = "hashed_password_3",
                UserType = UserTypeEnum.Provider
            },
            new User
            {
                Id = 4, Name = "Stijn", Email = "test@email.com", PasswordHash = "hashed_password_4",
                UserType = UserTypeEnum.Buyer
            });
        
        modelBuilder.Entity<Auctioneer>().HasData(
            new Auctioneer
            {
                Id = 1,UserId = 1
            },
            new Auctioneer()
            {
                Id = 2,UserId = 2
            },
            new Auctioneer()
            {
                Id = 3,UserId = 3
            },
            new Auctioneer()
            {
                Id = 4,UserId = 4
            });
    }
}
