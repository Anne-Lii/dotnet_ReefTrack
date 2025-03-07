using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReefTrack.Data;
using ReefTrack.Models;

namespace ReefTrack.Controllers
{
    [Authorize]
    public class FishController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FishController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Fish
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Fishes.Include(f => f.Aquarium);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Fish/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fish = await _context.Fishes
                .Include(f => f.Aquarium)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fish == null)
            {
                return NotFound();
            }

            return View(fish);
        }

        //GET: Fish/Create
        public IActionResult Create()
        {
            ViewData["AquariumId"] = new SelectList(_context.Aquariums, "Id", "Name");
            return View();
        }

        //POST: Fish/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CommonName,LatinName,Species,Quantity,AddedDate,AquariumId,ImageFile")] Fish fish)
        {
            if (ModelState.IsValid)
            {
                if (fish.ImageFile != null)
                {
                    // Skapa unikt filnamn
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fish.ImageFile.FileName);
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/fish", fileName);

                    using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await fish.ImageFile.CopyToAsync(fileStream);
                    }

                    // Spara filnamnet i databasen
                    fish.ImageName = fileName;
                }

                _context.Add(fish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AquariumId"] = new SelectList(_context.Aquariums, "Id", "Name", fish.AquariumId);
            return View(fish);
        }


        //GET: Fish/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fish = await _context.Fishes.FindAsync(id);
            if (fish == null)
            {
                return NotFound();
            }
            ViewData["AquariumId"] = new SelectList(_context.Aquariums, "Id", "Name", fish.AquariumId);
            return View(fish);
        }

        // POST: Fish/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CommonName,LatinName,Species,Quantity,AddedDate,AquariumId,ImageFile,ImageName")] Fish fish)
        {
            if (id != fish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (fish.ImageFile != null)
                    {
                        // Ta bort gammal bild om det finns en
                        if (!string.IsNullOrEmpty(fish.ImageName))
                        {
                            string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/fish", fish.ImageName);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Ladda upp ny bild
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fish.ImageFile.FileName);
                        string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/fish", fileName);

                        using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                        {
                            await fish.ImageFile.CopyToAsync(fileStream);
                        }

                        fish.ImageName = fileName;
                    }

                    _context.Update(fish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FishExists(fish.Id))
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

            ViewData["AquariumId"] = new SelectList(_context.Aquariums, "Id", "Name", fish.AquariumId);
            return View(fish);
        }


        // GET: Fish/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fish = await _context.Fishes
                .Include(f => f.Aquarium)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fish == null)
            {
                return NotFound();
            }

            return View(fish);
        }

        // POST: Fish/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fish = await _context.Fishes.FindAsync(id);
            if (fish != null)
            {
                _context.Fishes.Remove(fish);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FishExists(int id)
        {
            return _context.Fishes.Any(e => e.Id == id);
        }
    }
}
