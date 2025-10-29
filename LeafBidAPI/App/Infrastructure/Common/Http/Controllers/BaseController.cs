using LeafBidAPI.App.Infrastructure.Common.Data;
using Microsoft.AspNetCore.Mvc;

namespace LeafBidAPI.App.Infrastructure.Common.Http.Controllers;

public class BaseController(ApplicationDbContext dbContext)
{
    protected readonly ApplicationDbContext Context = dbContext;

    protected static ActionResult NotFound()
    {
        return new JsonResult(new { message = "Not Found" }) { StatusCode = 404 };
    }
    
    protected static ActionResult BadRequest(object errors)
    {
        return new JsonResult(new { errors }) { StatusCode = 400 };
    }
}