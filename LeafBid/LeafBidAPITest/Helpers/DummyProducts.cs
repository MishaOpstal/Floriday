using LeafBidAPI.Models;

namespace LeafBidAPITest.Helpers;

public class DummyProducts
{
    public static List<Product> GetFakeProducts()
    {
        return new List<Product>
        {
            new()
            {
                Id = 1,
                Name = "Rose Bouquet",
                Description = "A beautiful bouquet of red roses.",
                MinPrice = 1.34m,
                MaxPrice = 1.34m,
                Weight = 0.5,
                Species = "Rosa",
                Region = "Netherlands",
                PotSize = null,
                StemLength = 19,
                Stock = 50,
                HarvestedAt = DateTime.UtcNow.AddDays(-2),
                UserId = "user1"
            },
            new()
            {
                Id = 2,
                Name = "Tulip Bunch",
                Description = "A vibrant bunch of tulips in various colors.",
                MinPrice = 0.89m,
                MaxPrice = 0.89m,
                Weight = 0.3,
                Species = "Tulipa",
                Region = "Netherlands",
                PotSize = null,
                StemLength = 17,
                Stock = 100,
                HarvestedAt = DateTime.UtcNow.AddDays(-1),
                UserId = "user1"
            },
            new()
            {
                Id = 3,
                Name = "Potted Orchid",
                Description = "A delicate potted orchid plant.",
                MinPrice = 15.00m,
                MaxPrice = 15.00m,
                Weight = 1.2,
                Species = "Orchidaceae",
                Region = "Thailand",
                PotSize = 12,
                StemLength = null,
                Stock = 30,
                HarvestedAt = DateTime.UtcNow.AddDays(-5),
                UserId = "user1"
            },
            
            new()
            {
                Id = 4,
                Name = "Sunflower Bundle",
                Description = "A cheerful bundle of sunflowers.",
                MinPrice = 2.50m,
                MaxPrice = 2.50m,
                Weight = 0.8,
                Species = "Helianthus",
                Region = "Spain",
                PotSize = null,
                StemLength = 22,
                Stock = 75,
                HarvestedAt = DateTime.UtcNow.AddDays(-3),
                UserId = "user1"
            }
        };
    }
}