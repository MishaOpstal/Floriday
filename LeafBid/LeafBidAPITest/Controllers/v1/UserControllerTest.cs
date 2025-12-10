using System.Reflection;
using System.Security.Claims;
using LeafBidAPI.Controllers.v1;
using LeafBidAPI.Data;
using LeafBidAPI.DTOs.User;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
namespace LeafBidAPITest.Controllers.v1;

public class UserControllerTest
{
    private static ApplicationDbContext CreateDbContext()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        ApplicationDbContext dbContext = new(options);

        // Ensures IdentityDbContext tables & model are initialized for this in-memory database instance.
        dbContext.Database.EnsureCreated();

        return dbContext;
    }

    private static Mock<UserManager<User>> CreateUserManagerMock()
    {
        Mock<IUserStore<User>> store = new();

        // mock requires the nulls as these are in 'UserManager<TUser>', these are for objects used under the hood by IdentityFramework
        Mock<UserManager<User>> userManager = new(
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

        return userManager;
    }

    private static Mock<SignInManager<User>> CreateSignInManagerMock()
    {
        Mock<IUserStore<User>> store = new();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();
        Mock<IHttpContextAccessor> contextAccessor = new();
        Mock<IUserClaimsPrincipalFactory<User>> claimsFactory = new();

        Mock<SignInManager<User>> signInManager = new(
            userManagerMock.Object,
            contextAccessor.Object,
            claimsFactory.Object,
            null,
            null,
            null,
            null
        );

        return signInManager;
    }

    private static UserController CreateController(
        ApplicationDbContext dbContext,
        SignInManager<User> signInManager,
        UserManager<User> userManager,

        ClaimsPrincipal? user = null) // a claimsPrincipal is a .NET object that represents who the current user is)
    {
        UserController controller = new(
            dbContext,
            signInManager,
            userManager

        );

        DefaultHttpContext httpContext = new()
        {
            User = user ?? new ClaimsPrincipal(new ClaimsIdentity())
        };

        // assign the mock HTTP context to the controller
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        return controller;
    }

    private static string GetJsonMessage(JsonResult json)
    {
        object? value = json.Value;
        Assert.NotNull(value);

        PropertyInfo? messagePropertyInfo = value.GetType().GetProperty("Message");
        Assert.NotNull(messagePropertyInfo);

        object? messageValue = messagePropertyInfo.GetValue(value);
        Assert.NotNull(messageValue);

        return (string)messageValue;
    }

    [Fact]
    public async Task RegisterUser_WithValidData_CreatesUser()
    {
        // Arrange
        using ApplicationDbContext dbContext = CreateDbContext();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();
        Mock<SignInManager<User>> signInManagerMock = CreateSignInManagerMock();

        UserController controller = CreateController(
            dbContext,
            signInManagerMock.Object,
            userManagerMock.Object
        );

        userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

// If your controller uses AddToRolesAsync:
        userManagerMock.Setup(um => um.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);

// SignInManager.SignInAsync returns Task
        signInManagerMock.Setup(s => s.SignInAsync(It.IsAny<User>(), It.IsAny<bool>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

// Example: correct DTO initialization
        var createUserDto = new CreateUserDto
        {
            UserName = "testuser",
            Email = "test@test.com",
            Password = "1234Ab!",
            PasswordConfirmation = "1234Ab!",
            Roles = new[] { "Provider" }
        };
        ActionResult<User> actionResult = await controller.RegisterUser(createUserDto);

        // Assert
        JsonResult json = Assert.IsType<JsonResult>(actionResult.Result);
        Assert.Equal(StatusCodes.Status200OK, json.StatusCode);
        Assert.Equal("Registered Successfully.", GetJsonMessage(json));
    }
    
    [Fact]
    public async Task RegisterUser_WithMismatchedPasswords_ReturnsBadRequest()
    {
        // Arrange
        using ApplicationDbContext dbContext = CreateDbContext();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();
        Mock<SignInManager<User>> signInManagerMock = CreateSignInManagerMock();

        UserController controller = CreateController(
            dbContext,
            signInManagerMock.Object,
            userManagerMock.Object
        );

        userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

// If your controller uses AddToRolesAsync:
        userManagerMock.Setup(um => um.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);

// SignInManager.SignInAsync returns Task
        signInManagerMock.Setup(s => s.SignInAsync(It.IsAny<User>(), It.IsAny<bool>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

// Example: correct DTO initialization
        var createUserDto = new CreateUserDto
        {
            UserName = "testuser",
            Email = "test@test.com",
            Password = "1234Ab!",
            PasswordConfirmation = "PIZZA!",
            Roles = new[] { "Provider" }
        };
        ActionResult<User> actionResult = await controller.RegisterUser(createUserDto);

        // Assert
        JsonResult json = Assert.IsType<JsonResult>(actionResult.Result);
        Assert.Equal(StatusCodes.Status400BadRequest, json.StatusCode);
        Assert.Equal("Passwords do not match.", GetJsonMessage(json));
    }
    [Fact]
    public async Task LoginUser_WithValidCredentials_ReturnsOk()
    {
        // Arrange
        using ApplicationDbContext dbContext = CreateDbContext();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();
        Mock<SignInManager<User>> signInManagerMock = CreateSignInManagerMock();
        
        UserController controller = CreateController(
            dbContext,
            signInManagerMock.Object,
            userManagerMock.Object
        );

         var existingUser = new User
{
    UserName = "testuser",
    Email = "testuser@mail.gov",
    Id = Guid.NewGuid().ToString()
};

// make UserManager return the user when the controller looks it up by email
userManagerMock.Setup(um => um.FindByEmailAsync(existingUser.Email))
    .ReturnsAsync(existingUser);

// make SignInManager return success for password sign-in
signInManagerMock.Setup(s => s.PasswordSignInAsync(
        It.IsAny<User>(),
        It.IsAny<string>(),
        It.IsAny<bool>(),
        It.IsAny<bool>()))
    .ReturnsAsync(SignInResult.Success);

// (optional) keep CreateAsync mock if you still call RegisterUser
userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
    .ReturnsAsync(IdentityResult.Success);
        
         await controller.RegisterUser(createUserDto);
        
        string userEmail = "testuser@mail.gov";
        string password = "1234Ab!";
        
        LoginUserDto loginUserDto = new()
        {
            Email = userEmail,
            Password = password,
            Remember = true
        };
        
        ActionResult actionResult = (ActionResult)await controller.LoginUser(loginUserDto);

        // Assert
        JsonResult json = Assert.IsType<JsonResult>(actionResult);
        Assert.Equal("Epic", GetJsonMessage(json));
        Assert.Equal(StatusCodes.Status200OK, json.StatusCode);
    }
    [Fact]
    public async Task LoginUser_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        using ApplicationDbContext dbContext = CreateDbContext();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();
        Mock<SignInManager<User>> signInManagerMock = CreateSignInManagerMock();

        UserController controller = CreateController(
            dbContext,
            signInManagerMock.Object,
            userManagerMock.Object
        );

        CreateUserDto createUserDto = new()
        {
            UserName = "testuser",
            Email = "testuser@mail.gov",
            Password = "1234Ab!",
            PasswordConfirmation = "1234Ab!",
            Roles = ["Provider"]
        };
        
        string userEmail = "testuser@mail.gov";
        string password = "1234Ab!";
        
        LoginUserDto loginUserDto = new()
        {
            Email = userEmail,
            Password = password,
            Remember = false
        };
        
        IActionResult actionResult = await controller.LoginUser(loginUserDto);

        // Assert
        JsonResult json = Assert.IsType<JsonResult>(actionResult);
        Assert.Equal(StatusCodes.Status401Unauthorized, json.StatusCode);
        Assert.Equal("Not authorized", GetJsonMessage(json));

        int auctionCount = await dbContext.Users.CountAsync();
        Assert.Equal(0, auctionCount);
    }
    [Fact]
    public async Task LogOutUser_LogsOutSuccessfully()
    {
        // Arrange
        using ApplicationDbContext dbContext = CreateDbContext();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();
        Mock<SignInManager<User>> signInManagerMock = CreateSignInManagerMock();

        UserController controller = CreateController(
            dbContext,
            signInManagerMock.Object,
            userManagerMock.Object
        );
    }
    [Fact]
    public async Task GetUser_ReturnsUser_WhenUserExists()
    {
        // Arrange
        using ApplicationDbContext dbContext = CreateDbContext();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();
        Mock<SignInManager<User>> signInManagerMock = CreateSignInManagerMock();

        UserController controller = CreateController(
            dbContext,
            signInManagerMock.Object,
            userManagerMock.Object
        );

        User user = new()
        {
            UserName = "existinguser",
            Email = "real@mail.com",
            Id = Guid.NewGuid().ToString()
        };
    }
    [Fact]
    public async Task GetUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        using ApplicationDbContext dbContext = CreateDbContext();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();
        Mock<SignInManager<User>> signInManagerMock = CreateSignInManagerMock();

        UserController controller = CreateController(
            dbContext,
            signInManagerMock.Object,
            userManagerMock.Object
        );

        string nonExistentUserId = Guid.NewGuid().ToString();
    }
    
    [Fact]
    public async Task GetUsersById_ReturnsUser_WhenUserExists()
    {
        // Arrange
        using ApplicationDbContext dbContext = CreateDbContext();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();
        Mock<SignInManager<User>> signInManagerMock = CreateSignInManagerMock();

        UserController controller = CreateController(
            dbContext,
            signInManagerMock.Object,
            userManagerMock.Object
        );

        User user = new()
        {
            UserName = "uniqueuser",
            Email = "wow@mail.com",
            Id = Guid.NewGuid().ToString()
        };
    }
    [Fact]
    public async Task GetUsersById_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        using ApplicationDbContext dbContext = CreateDbContext();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();
        Mock<SignInManager<User>> signInManagerMock = CreateSignInManagerMock();

        UserController controller = CreateController(
            dbContext,
            signInManagerMock.Object,
            userManagerMock.Object
        );

        string nonExistentUserId = Guid.NewGuid().ToString();
    }
    [Fact]
    public async Task UpdateUserRoles_WithValidRoles_UpdatesSuccessfully()
    {
        // Arrange
        using ApplicationDbContext dbContext = CreateDbContext();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();
        Mock<SignInManager<User>> signInManagerMock = CreateSignInManagerMock();

        UserController controller = CreateController(
            dbContext,
            signInManagerMock.Object,
            userManagerMock.Object
        );

        User user = new()
        {
            UserName = "roleuser",
            Email = "lol@mail.com",
            Id = Guid.NewGuid().ToString()
        };
    }
    [Fact]
    public async Task UpdateUserRoles_WithInvalidRoles_ReturnsBadRequest()
    {
        // Arrange
        using ApplicationDbContext dbContext = CreateDbContext();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();
        Mock<SignInManager<User>> signInManagerMock = CreateSignInManagerMock();

        UserController controller = CreateController(
            dbContext,
            signInManagerMock.Object,
            userManagerMock.Object
        );

        User user = new()
        {
            UserName = "invalidroleuser",
            Email = "cool@mail.com",
            Id = Guid.NewGuid().ToString()
        };
    }
    [Fact]
    public async Task UpdateUserRoles_UserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        using ApplicationDbContext dbContext = CreateDbContext();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();
        Mock<SignInManager<User>> signInManagerMock = CreateSignInManagerMock();

        UserController controller = CreateController(
            dbContext,
            signInManagerMock.Object,
            userManagerMock.Object
        );

        string nonExistentUserId = Guid.NewGuid().ToString();
    }
    [Fact]
    public async Task DeleteUser_UserExists_DeletesSuccessfully()
    {
        // Arrange
        using ApplicationDbContext dbContext = CreateDbContext();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();
        Mock<SignInManager<User>> signInManagerMock = CreateSignInManagerMock();

        UserController controller = CreateController(
            dbContext,
            signInManagerMock.Object,
            userManagerMock.Object
        );

        User user = new()
        {
            UserName = "deleteuser",
            Email = "alot@mail.com",
            Id = Guid.NewGuid().ToString()
        };
    }
    [Fact]
    public async Task DeleteUser_UserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        using ApplicationDbContext dbContext = CreateDbContext();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();
        Mock<SignInManager<User>> signInManagerMock = CreateSignInManagerMock();

        UserController controller = CreateController(
            dbContext,
            signInManagerMock.Object,
            userManagerMock.Object
        );

        string nonExistentUserId = Guid.NewGuid().ToString();
    }
}
