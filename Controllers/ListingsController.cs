using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurMarketBackend.Data;
using OurMarketBackend.models;
using OurMarketBackend.Models;
using OurMarketBackend.Services;
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
            var query = _context.Listings
                .Include(l => l.Images)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var t = q.Trim().ToLower();
                query = query.Where(l =>
                    (l.Title ?? "").ToLower().Contains(t) ||
                    (l.Description ?? "").ToLower().Contains(t));
            }
            if (!string.IsNullOrWhiteSpace(category)) query = query.Where(l => l.Category == category);
            if (!string.IsNullOrWhiteSpace(location))
            {
                var t = location.Trim().ToLower();
                query = query.Where(l => (l.Location ?? "").ToLower().Contains(t));
            }


            return View(query.OrderByDescending(l => l.Id).ToList());
        }

        // GET: /Listings/Details/5
        public IActionResult Details(int id)
        {
            var listing = _context.Listings
                                  .Include(l => l.User)
                                  .Include(l => l.Images)
                                  .FirstOrDefault(l => l.Id == id);
            if (listing == null) return NotFound();
            return View(listing);
        }

        // GET: /Listings/Create
        [Authorize]
        public IActionResult Create() => View();

        // POST: /Listings/Create
        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ListingCreateViewModel vm, [FromServices] IZipLookup zipLookup)
        {
            // resolve City/State from ZIP (server-side safety)
            if (zipLookup.TryGet(vm.Zip, out var info))
            {
                vm.City = info.City;
                vm.State = info.State;
            }

            if (!ModelState.IsValid) return View(vm);

            var listing = new Listing
            {
                Title = vm.Title!,
                Description = vm.Description ?? "",
                Category = vm.Category ?? "",
                Price = vm.Price,
                UserId = _userManager.GetUserId(User)!,
                Location = $"{vm.City}, {vm.State}"   // <— set automatically
            };

            // save images to wwwroot/images/listings (your existing loop)
            var root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "listings");
            Directory.CreateDirectory(root);
            foreach (var file in vm.Photos!.Take(3))
            {
                var ext = Path.GetExtension(file.FileName);
                var name = $"{Guid.NewGuid():N}{ext}";
                var savePath = Path.Combine(root, name);

                using var stream = System.IO.File.Create(savePath);
                await file.CopyToAsync(stream);

                listing.Images.Add(new ListingImage { FileName = name });
            }


            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = listing.Id });
        }


    }
}
