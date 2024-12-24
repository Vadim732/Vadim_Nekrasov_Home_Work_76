using EstablishmentRating.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        var establishment = _context.Establishments.Include(a => a.Reviews).FirstOrDefault(e => e.Id == id);
        return View(establishment);
    }
    [HttpPost]
    public IActionResult AddReview(Review review)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors });
        }

        _context.Reviews.Add(review);
        _context.SaveChanges();
        var newReview = new
        {
            title = review.Title,
            description = review.Description,
            stars = review.Stars
        };

        return Json(new { success = true, review = newReview });
    }

}