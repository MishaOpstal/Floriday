using LeafBidAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace LeafBidAPI.Controllers;

public class BaseController(ApplicationDbContext dbContext)
{
    protected readonly ApplicationDbContext Context = dbContext;
    
    protected static ActionResult BadRequest(string message = "Bad request")
    {
        return new JsonResult(new { message }) { StatusCode = 400 };
    }

    protected static ActionResult NotFound(string message = "Not Found")
    {
        return new JsonResult(new { message = "Not Found" }) { StatusCode = 404 };
    }
}