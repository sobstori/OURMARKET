using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurMarketBackend.Data;
using OurMarketBackend.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OurMarketBackend.Controllers
{
    public class ListingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ListingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Listings
        public IActionResult Index(string? q, string? category, string? location)
        {
            var query = _context.Listings.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var t = q.Trim().ToLower();
                query = query.Where(l =>
                    (l.Title ?? "").ToLower().Contains(t) ||
                    (l.Description ?? "").ToLower().Contains(t));
            }
            if (!string.IsNullOrWhiteSpace(category)) query = query.Where(l => l.Category == category);
            if (!string.IsNullOrWhiteSpace(location)) query = query.Where(l => l.Location == location);

            return View(query.OrderByDescending(l => l.Id).ToList());
        }

        // GET: /Listings/Details/5
        public IActionResult Details(int id)
        {
            var listing = _context.Listings
                                  .Include(l => l.User)
                                  .FirstOrDefault(l => l.Id == id);
            if (listing == null) return NotFound();
            return View(listing);
        }

        // GET: /Listings/Create
        [Authorize]
        public IActionResult Create() => View();

        // POST: /Listings/Create
        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,Description,Price,Location,ImageUrl,Category")] Listing listing)
        {
            // UserId isn't posted; remove it from ModelState before validating
            ModelState.Remove(nameof(Listing.UserId));

            if (!ModelState.IsValid) return View(listing);

            listing.UserId = _userManager.GetUserId(User)!; // link to current user
            listing.User = null; // avoid accidental attach

            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
