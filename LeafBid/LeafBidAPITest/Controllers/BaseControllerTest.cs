using System.Reflection;
using LeafBidAPI.Controllers;
using LeafBidAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPITest.Controllers;

public class BaseControllerTest
{
    private static ApplicationDbContext CreateDbContext()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static void AssertJsonResult(ActionResult result, int expectedStatusCode, string expectedMessage)
    {
        JsonResult json = Assert.IsType<JsonResult>(result);
        Assert.Equal(expectedStatusCode, json.StatusCode);

        object? value = json.Value;
        Assert.NotNull(value);

        PropertyInfo? messagePropertyInfo = value.GetType().GetProperty("Message");
        Assert.NotNull(messagePropertyInfo);

        object? messageValue = messagePropertyInfo.GetValue(value);
        Assert.NotNull(messageValue);

        Assert.Equal(expectedMessage, messageValue);
    }

    [Fact]
    public void OkResult_WithMessage_Returns200_AndMessage()
    {
        using ApplicationDbContext dbContext = CreateDbContext();
        BaseController controller = new(dbContext);

        const string message = "This is a test message";

        ActionResult result = controller.OkResult(message);

        AssertJsonResult(result, 200, message);
    }

    [Fact]
    public void OkResult_WithoutMessage_Returns200_AndDefaultMessage()
    {
        using ApplicationDbContext dbContext = CreateDbContext();
        BaseController controller = new(dbContext);

        ActionResult result = controller.OkResult();

        AssertJsonResult(result, 200, "Success");
    }

    [Fact]
    public void BadRequest_WithMessage_Returns400_AndMessage()
    {
        using ApplicationDbContext dbContext = CreateDbContext();
        BaseController controller = new(dbContext);

        const string message = "Nope";

        ActionResult result = controller.BadRequest(message);

        AssertJsonResult(result, 400, message);
    }

    [Fact]
    public void BadRequest_WithoutMessage_Returns400_AndDefaultMessage()
    {
        using ApplicationDbContext dbContext = CreateDbContext();
        BaseController controller = new(dbContext);

        ActionResult result = controller.BadRequest();

        AssertJsonResult(result, 400, "Bad request");
    }

    [Fact]
    public void InternalError_WithMessage_Returns500_AndMessage()
    {
        using ApplicationDbContext dbContext = CreateDbContext();
        BaseController controller = new(dbContext);

        const string message = "Something exploded";

        ActionResult result = controller.InternalError(message);

        AssertJsonResult(result, 500, message);
    }

    [Fact]
    public void InternalError_WithoutMessage_Returns500_AndDefaultMessage()
    {
        using ApplicationDbContext dbContext = CreateDbContext();
        BaseController controller = new(dbContext);

        ActionResult result = controller.InternalError();

        AssertJsonResult(result, 500, "Internal Server Error");
    }

    [Fact]
    public void NotFound_WithMessage_Returns404_AndMessage()
    {
        using ApplicationDbContext dbContext = CreateDbContext();
        BaseController controller = new(dbContext);

        const string message = "Where did it go?";

        ActionResult result = controller.NotFound(message);

        AssertJsonResult(result, 404, message);
    }

    [Fact]
    public void NotFound_WithoutMessage_Returns404_AndDefaultMessage()
    {
        using ApplicationDbContext dbContext = CreateDbContext();
        BaseController controller = new(dbContext);

        ActionResult result = controller.NotFound();

        AssertJsonResult(result, 404, "Not Found");
    }

    [Fact]
    public void Unauthorized_WithMessage_Returns401_AndMessage()
    {
        using ApplicationDbContext dbContext = CreateDbContext();
        BaseController controller = new(dbContext);

        const string message = "No access";

        ActionResult result = controller.Unauthorized(message);

        AssertJsonResult(result, 401, message);
    }

    [Fact]
    public void Unauthorized_WithoutMessage_Returns401_AndDefaultMessage()
    {
        using ApplicationDbContext dbContext = CreateDbContext();
        BaseController controller = new(dbContext);

        ActionResult result = controller.Unauthorized();

        AssertJsonResult(result, 401, "Not authorized");
    }
}
