using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReefTrack.Data;
using ReefTrack.Models;

namespace ReefTrack.Controllers
{
    public class CoralController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoralController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Coral
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Corals.Include(c => c.Aquarium);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Coral/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coral = await _context.Corals
                .Include(c => c.Aquarium)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coral == null)
            {
                return NotFound();
            }

            return View(coral);
        }

        // GET: Coral/Create
        public IActionResult Create()
        {
            ViewData["AquariumId"] = new SelectList(_context.Aquariums, "Id", "Name");
            return View();
        }

        //POST: Coral/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CommonName,LatinName,Species,Quantity,AddedDate,AquariumId,ImageFile")] Coral coral)
        {
            if (ModelState.IsValid)
            {
                // Hantera bilduppladdning om en bild är vald
                if (coral.ImageFile != null)
                {
                    // Skapa unikt filnamn för att undvika namnkonflikter
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(coral.ImageFile.FileName);
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/coral", fileName);

                    // Ladda upp filen till servern
                    using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await coral.ImageFile.CopyToAsync(fileStream);
                    }

                    // Spara filnamnet i databasen
                    coral.ImageName = fileName;
                }

                // Lägg till korallen i databasen
                _context.Add(coral);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Återvänd till vyn om modellen inte är giltig
            ViewData["AquariumId"] = new SelectList(_context.Aquariums, "Id", "Name", coral.AquariumId);
            return View(coral);
        }


        // GET: Coral/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coral = await _context.Corals.FindAsync(id);
            if (coral == null)
            {
                return NotFound();
            }
            ViewData["AquariumId"] = new SelectList(_context.Aquariums, "Id", "Name", coral.AquariumId);
            return View(coral);
        }

        // POST: Coral/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CommonName,LatinName,Species,Quantity,AddedDate,AquariumId")] Coral coral)
        {
            if (id != coral.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coral);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoralExists(coral.Id))
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
            ViewData["AquariumId"] = new SelectList(_context.Aquariums, "Id", "Name", coral.AquariumId);
            return View(coral);
        }

        // GET: Coral/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coral = await _context.Corals
                .Include(c => c.Aquarium)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coral == null)
            {
                return NotFound();
            }

            return View(coral);
        }

        // POST: Coral/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coral = await _context.Corals.FindAsync(id);
            if (coral != null)
            {
                _context.Corals.Remove(coral);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoralExists(int id)
        {
            return _context.Corals.Any(e => e.Id == id);
        }
    }
}
