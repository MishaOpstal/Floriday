using LeafBidAPI.Models;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace LeafBidAPITest.Helpers;

public class dummyUsers
{
    
    public static UserManager<User> CreateFakeUserManager()
    {
        Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();

        return new UserManager<User>(
            store.Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );
    }

    public static UserManager<User> CreateFakeAuctioneerUserManager()
    {
        Mock<IUserRoleStore<User>> store = new Mock<IUserRoleStore<User>>();
        
        User user = new User { UserName = "auctioneer" };
        store.Setup(s => s.GetRolesAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<string> { "Auctioneer" });

        return new UserManager<User>(
            store.Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );
    }

}