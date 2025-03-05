using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReefTrack.Data;
using Reeftrack.Models;

namespace ReefTrack.Controllers
{
    public class AquariumController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnviroment;
        private readonly string wwwRootPath;

        public AquariumController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnviroment = hostEnvironment;
            wwwRootPath = hostEnvironment.WebRootPath;
        }

        //GET: Aquarium
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Aquariums.Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        //GET: Aquarium/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Aquariums == null)
            {
                return NotFound();
            }

            var aquarium = await _context.Aquariums
                .Include(navigationPropertyPath: a => a.Fishes)
                .Include(navigationPropertyPath: a => a.Corals)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (aquarium == null)
            {
                return NotFound();
            }

            return View(aquarium);
        }

        //GET: Aquarium/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        //POST: Aquarium/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Size,Type,StartDate,UserId,ImageFile")] Aquarium aquarium)
        {
            if (ModelState.IsValid)
            {
                //Om det finns en bild
                if (aquarium.ImageFile != null)
                {
                    // Skapa unikt filnamn
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(aquarium.ImageFile.FileName);

                    // Sökväg till wwwroot/images
                    string uploadPath = Path.Combine(wwwRootPath, "images", fileName);

                    //spara filen i filsystemet
                    using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await aquarium.ImageFile.CopyToAsync(fileStream);
                    }

                    // Spara filnamnet i databasen
                    aquarium.ImageName = fileName;
                }else {
                    aquarium.ImageName = "empty.jpg";
                }

                _context.Add(aquarium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", aquarium.UserId);
            return View(aquarium);
        }


        //GET: Aquarium/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aquarium = await _context.Aquariums.FindAsync(id);
            if (aquarium == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", aquarium.UserId);
            return View(aquarium);
        }

        //POST: Aquarium/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Size,Type,StartDate,UserId,ImageFile,ImageName")] Aquarium aquarium)
        {
            if (id != aquarium.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (aquarium.ImageFile != null)
                    {
                        // Skapa nytt filnamn
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(aquarium.ImageFile.FileName);
                        string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                        {
                            await aquarium.ImageFile.CopyToAsync(fileStream);
                        }

                        // Ta bort gammal bild om det finns en
                        if (!string.IsNullOrEmpty(aquarium.ImageName))
                        {
                            string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", aquarium.ImageName);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Uppdatera databasfältet
                        aquarium.ImageName = fileName;
                    }

                    _context.Update(aquarium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AquariumExists(aquarium.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", aquarium.UserId);
            return View(aquarium);
        }

        //GET: Aquarium/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aquarium = await _context.Aquariums
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aquarium == null)
            {
                return NotFound();
            }

            return View(aquarium);
        }

        //POST: Aquarium/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aquarium = await _context.Aquariums.FindAsync(id);
            if (aquarium != null)
            {
                // Radera bildfil om den finns
                if (!string.IsNullOrEmpty(aquarium.ImageName))
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", aquarium.ImageName);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Aquariums.Remove(aquarium);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AquariumExists(int id)
        {
            return _context.Aquariums.Any(e => e.Id == id);
        }
    }
}
