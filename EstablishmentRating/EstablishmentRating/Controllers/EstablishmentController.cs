using EstablishmentRating.Models;
using EstablishmentRating.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstablishmentRating.Controllers;

public class EstablishmentController : Controller
{
    private readonly EstablishmentRatingContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IWebHostEnvironment _environment;

    public EstablishmentController(UserManager<User> userManager, EstablishmentRatingContext context, IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _context = context;
        _environment = environment;
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
    public async Task<IActionResult> CreateEstablishment(CreateEstablishmentViewModel model)
    {
        if (ModelState.IsValid)
        {
            string fileName = $"establishment_{model.Name}{Path.GetExtension(model.Image.FileName)}";
            
            if (model.Image != null && model.Image.Length > 0 && model.Image.ContentType.StartsWith("image/"))
            {
                string filePath = Path.Combine(_environment.WebRootPath, "establishmentImages", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
            }

            Establishment establishment = new Establishment()
            {
                Name = model.Name,
                Image = $"/establishmentImages/{fileName}",
                Description = model.Description
            };
            await _context.Establishments.AddAsync(establishment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(model);
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