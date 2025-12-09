using System.Diagnostics.CodeAnalysis;
using LeafBidAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace LeafBidAPI.Controllers;

[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
public class BaseController(ApplicationDbContext dbContext) : ControllerBase
{
    protected readonly ApplicationDbContext Context = dbContext;

    protected ActionResult OkResult(string message = "Success")
    {
        return new JsonResult(new { Message = message }) { StatusCode = 200 };
    }
    
    protected ActionResult BadRequest(string message = "Bad request")
    {
        return new JsonResult(new { Message = message }) { StatusCode = 400 };
    }

    protected ActionResult InternalError(string message = "Internal Server Error")
    {
        return new JsonResult(new { Message = message }) { StatusCode = 500 };
    }

    protected ActionResult NotFound(string message = "Not Found")
    {
        return new JsonResult(new { Message = message }) { StatusCode = 404 };
    }
    
    protected ActionResult Unauthorized(string message = "Not authorized")
    {
        return new JsonResult(new { Message = message }) { StatusCode = 401 };
    }
}