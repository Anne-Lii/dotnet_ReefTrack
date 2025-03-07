using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReefTrack.Data;
using ReefTrack.Models;
using System.Diagnostics;

namespace ReefTrack.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var aquariums = await _context.Aquariums.ToListAsync(); // Hämta alla akvarier
        return View(aquariums);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
