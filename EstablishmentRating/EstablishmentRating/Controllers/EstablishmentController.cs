using EstablishmentRating.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EstablishmentRating.Controllers;

public class EstablishmentController : Controller
{
    private readonly EstablishmentRatingContext _context;
    private readonly UserManager<User> _userManager;

    public EstablishmentController(UserManager<User> userManager, EstablishmentRatingContext context)
    {
        _userManager = userManager;
        _context = context;
    }
    public IActionResult Index()
    {
        var establishments = _context.Establishments.ToList();
        return View(establishments);
    }

    public IActionResult CreateEstablishment()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateEstablishment(Establishment establishment)
    {
        if (ModelState.IsValid)
        {
            _context.Establishments.Add(establishment);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(establishment);
    }
    public IActionResult Details(int id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var establishment = _context.Establishments.FirstOrDefault(e => e.Id == id);
        return View(establishment);
    }
}