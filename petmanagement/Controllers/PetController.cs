using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using petmanagement.Data;
using petmanagement.Models;
using petmanagement.Models.ViewModels;

namespace petmanagement.Controllers
{
    public class PetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PetController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pet/Browse
        public async Task<IActionResult> Browse(string searchString, string breed, int? age)
        {
            var petsQuery = _context.Pets.Where(p => !p.IsAdopted);

            if (!string.IsNullOrEmpty(searchString))
            {
                petsQuery = petsQuery.Where(p => p.Name.Contains(searchString) || 
                                                p.Breed.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(breed))
            {
                petsQuery = petsQuery.Where(p => p.Breed == breed);
            }

            if (age.HasValue)
            {
                petsQuery = petsQuery.Where(p => p.Age == age.Value);
            }

            var pets = await petsQuery.ToListAsync();
            
            ViewBag.SearchString = searchString;
            ViewBag.Breed = breed;
            ViewBag.Age = age;
            ViewBag.Breeds = await _context.Pets
                .Where(p => !p.IsAdopted)
                .Select(p => p.Breed)
                .Distinct()
                .ToListAsync();

            return View(pets);
        }

        // GET: Pet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .FirstOrDefaultAsync(m => m.PetID == id && !m.IsAdopted);

            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // GET: Pet/Adopt/5
        public async Task<IActionResult> Adopt(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .FirstOrDefaultAsync(m => m.PetID == id && !m.IsAdopted);

            if (pet == null)
            {
                return NotFound();
            }

            var viewModel = new AdoptionRequestViewModel
            {
                PetID = pet.PetID,
                PetName = pet.Name,
                PetImagePath = pet.ImagePath
            };

            return View(viewModel);
        }

        // POST: Pet/Adopt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adopt(AdoptionRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if user exists, if not create one
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null)
                {
                    user = new User
                    {
                        Name = model.Name,
                        Email = model.Email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("TempPassword123!"),
                        Role = "User"
                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }

                var adoptionRequest = new AdoptionRequest
                {
                    UserID = user.UserID,
                    PetID = model.PetID,
                    Message = model.Message,
                    Status = "Pending"
                };

                _context.AdoptionRequests.Add(adoptionRequest);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Your adoption request has been submitted successfully! We'll contact you soon.";
                return RedirectToAction("Details", new { id = model.PetID });
            }

            // If we got this far, something failed, redisplay form
            var pet = await _context.Pets.FindAsync(model.PetID);
            if (pet != null)
            {
                model.PetName = pet.Name;
                model.PetImagePath = pet.ImagePath;
            }

            return View(model);
        }
    }
}