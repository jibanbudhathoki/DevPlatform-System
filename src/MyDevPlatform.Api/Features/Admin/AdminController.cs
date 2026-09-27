using Microsoft.AspNetCore.Mvc;

namespace MyDevPlatform.Api.Features.Admin;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View("~/Views/admin/Pages/Index.cshtml");
    }
}