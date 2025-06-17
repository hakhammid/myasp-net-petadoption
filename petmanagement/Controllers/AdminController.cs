using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using petmanagement.Data;
using petmanagement.Models;
using petmanagement.Models.ViewModels;
using petmanagement.Services;

namespace petmanagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public AdminController(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var stats = new
            {
                TotalPets = await _context.Pets.CountAsync(),
                AvailablePets = await _context.Pets.CountAsync(p => !p.IsAdopted),
                AdoptedPets = await _context.Pets.CountAsync(p => p.IsAdopted),
                PendingRequests = await _context.AdoptionRequests.CountAsync(r => r.Status == "Pending")
            };

            ViewBag.Stats = stats;
            return View();
        }

        // GET: Admin/Pets
        public async Task<IActionResult> Pets()
        {
            var pets = await _context.Pets.OrderByDescending(p => p.CreatedDate).ToListAsync();
            return View(pets);
        }

        // GET: Admin/CreatePet
        public IActionResult CreatePet()
        {
            return View();
        }

        // POST: Admin/CreatePet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePet(PetCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pet = new Pet
                {
                    Name = model.Name,
                    Breed = model.Breed,
                    Age = model.Age,
                    Gender = model.Gender,
                    Description = model.Description,
                    IsAdopted = false,
                    CreatedDate = DateTime.UtcNow
                };

                if (model.ImageFile != null)
                {
                    try
                    {
                        pet.ImagePath = await _fileService.SaveFileAsync(model.ImageFile, "pets");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ImageFile", ex.Message);
                        return View(model);
                    }
                }

                _context.Pets.Add(pet);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Pet created successfully!";
                return RedirectToAction(nameof(Pets));
            }

            return View(model);
        }

        // GET: Admin/EditPet/5
        public async Task<IActionResult> EditPet(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            var model = new PetCreateViewModel
            {
                PetID = pet.PetID,
                Name = pet.Name,
                Breed = pet.Breed,
                Age = pet.Age,
                Gender = pet.Gender,
                Description = pet.Description,
                ExistingImagePath = pet.ImagePath
            };

            return View(model);
        }

        // POST: Admin/EditPet/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPet(int id, PetCreateViewModel model)
        {
            if (id != model.PetID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var pet = await _context.Pets.FindAsync(id);
                    if (pet == null)
                    {
                        return NotFound();
                    }

                    pet.Name = model.Name;
                    pet.Breed = model.Breed;
                    pet.Age = model.Age;
                    pet.Gender = model.Gender;
                    pet.Description = model.Description;

                    if (model.ImageFile != null)
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(pet.ImagePath))
                        {
                            _fileService.DeleteFile(pet.ImagePath);
                        }

                        pet.ImagePath = await _fileService.SaveFileAsync(model.ImageFile, "pets");
                    }

                    _context.Update(pet);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Pet updated successfully!";
                    return RedirectToAction(nameof(Pets));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetExists(model.PetID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ImageFile", ex.Message);
                }
            }

            model.ExistingImagePath = model.ExistingImagePath;
            return View(model);
        }

        // GET: Admin/DeletePet/5
        public async Task<IActionResult> DeletePet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .FirstOrDefaultAsync(m => m.PetID == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // POST: Admin/DeletePet/5
        [HttpPost, ActionName("DeletePet")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePetConfirmed(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet != null)
            {
                // Delete associated image file
                if (!string.IsNullOrEmpty(pet.ImagePath))
                {
                    _fileService.DeleteFile(pet.ImagePath);
                }

                _context.Pets.Remove(pet);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Pet deleted successfully!";
            }

            return RedirectToAction(nameof(Pets));
        }

        // GET: Admin/AdoptionRequests
        public async Task<IActionResult> AdoptionRequests()
        {
            var requests = await _context.AdoptionRequests
                .Include(r => r.User)
                .Include(r => r.Pet)
                .OrderByDescending(r => r.DateRequested)
                .ToListAsync();

            return View(requests);
        }

        // POST: Admin/UpdateRequestStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRequestStatus(int requestId, string status)
        {
            var request = await _context.AdoptionRequests
                .Include(r => r.Pet)
                .FirstOrDefaultAsync(r => r.RequestID == requestId);

            if (request != null)
            {
                request.Status = status;

                // If approved, mark pet as adopted
                if (status == "Approved")
                {
                    request.Pet.IsAdopted = true;
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Request {status.ToLower()} successfully!";
            }

            return RedirectToAction(nameof(AdoptionRequests));
        }

        private bool PetExists(int id)
        {
            return _context.Pets.Any(e => e.PetID == id);
        }
    }
}
