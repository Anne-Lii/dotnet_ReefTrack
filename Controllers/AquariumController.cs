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

        public AquariumController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Aquarium
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Aquariums.Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Aquarium/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Aquarium/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Aquarium/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Size,Type,StartDate,UserId")] Aquarium aquarium)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aquarium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", aquarium.UserId);
            return View(aquarium);
        }

        // GET: Aquarium/Edit/5
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

        // POST: Aquarium/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Size,Type,StartDate,UserId")] Aquarium aquarium)
        {
            if (id != aquarium.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        // GET: Aquarium/Delete/5
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

        // POST: Aquarium/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aquarium = await _context.Aquariums.FindAsync(id);
            if (aquarium != null)
            {
                _context.Aquariums.Remove(aquarium);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AquariumExists(int id)
        {
            return _context.Aquariums.Any(e => e.Id == id);
        }
    }
}
