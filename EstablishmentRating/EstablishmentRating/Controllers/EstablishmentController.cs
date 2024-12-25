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
    public IActionResult Index(int page = 1)
    {
        int pageSize = 8;
        var establishments = _context.Establishments.ToList();
        return View(establishments);
    }
    
    [HttpGet]
    public IActionResult Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return Json(new { success = true, establishments = new List<Establishment>() });
        }
        var results = _context.Establishments
            .Where(e => e.Name.Contains(query) || e.Description.Contains(query))
            .Select(e => new
            {
                e.Id,
                e.Name,
                e.Image
            })
            .ToList();

        return Json(new { success = true, establishments = results });
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

    [HttpPost]
    public async Task<IActionResult> UploadGalleryImage(IFormFile file, int id)
    {
        if (ModelState.IsValid)
        {
            Establishment establishment = await _context.Establishments.FirstOrDefaultAsync(e => e.Id == id);
        
            if (file != null && file.Length > 0 && file.ContentType.StartsWith("image/"))
            {
                string fileName = $"establishment_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                string filePath = Path.Combine(_environment.WebRootPath, "establishmentImages/gallery", fileName);
                
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                
                GalleryImage galleryImage = new GalleryImage()
                {
                    ImagePath = $"/establishmentImages/gallery/{fileName}",
                    EstablishmentId = establishment.Id
                };
                await _context.GalleryImages.AddAsync(galleryImage);
                await _context.SaveChangesAsync();
            }
        
            return RedirectToAction("Details", new { id = establishment.Id });
        }

        return NotFound();
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

    [HttpGet]
    public async Task<IActionResult> GetRating(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }
        Establishment establishment = await _context.Establishments.Include(e=> e.Reviews).FirstOrDefaultAsync(e => e.Id == id);
        if (establishment == null)
        {
            return NotFound();
        }
        List<Review> reviews = establishment.Reviews.ToList();
        double average = 0;
        if (reviews != null)
        {
            List<int> allRatings = new List<int>();
            for (int i = 0; i < reviews.Count; i++)
            {
               allRatings.Add(reviews[i].Stars);
            }
            average = allRatings.Average();
            average = Math.Round(average, 1);
        }

        return PartialView("_RatingPartialView", average);
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