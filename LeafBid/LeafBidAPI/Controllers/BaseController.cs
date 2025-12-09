using System.Diagnostics.CodeAnalysis;
using LeafBidAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace LeafBidAPI.Controllers;

[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
public class BaseController(ApplicationDbContext dbContext) : ControllerBase
{
    protected readonly ApplicationDbContext Context = dbContext;

    [NonAction]
    public ActionResult OkResult(string message = "Success")
    {
        return new JsonResult(new { Message = message }) { StatusCode = 200 };
    }
    
    [NonAction]
    public ActionResult BadRequest(string message = "Bad request")
    {
        return new JsonResult(new { Message = message }) { StatusCode = 400 };
    }

    [NonAction]
    public ActionResult InternalError(string message = "Internal Server Error")
    {
        return new JsonResult(new { Message = message }) { StatusCode = 500 };
    }

    [NonAction]
    public ActionResult NotFound(string message = "Not Found")
    {
        return new JsonResult(new { Message = message }) { StatusCode = 404 };
    }
    
    [NonAction]
    public ActionResult Unauthorized(string message = "Not authorized")
    {
        return new JsonResult(new { Message = message }) { StatusCode = 401 };
    }
}