using Microsoft.AspNetCore.Mvc;

namespace EstablishmentRating.Controllers;

public class EstablishmentController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}