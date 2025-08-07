using Microsoft.AspNetCore.Mvc;
using OURMARKET.data;
using OURMARKET.models;
using System.Linq;

namespace OURMARKET.Controllers
{
    public class ListingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Listings
        public IActionResult Index()
        {
            // Pull all listings from the database
            var listings = _context.Listings.ToList();
            return View(listings);
        }

        // GET: /Listings/Create
        public IActionResult Create()
        {
            // Show the post form
            return View();
        }

        // POST: /Listings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Listing listing)
        {
            if (ModelState.IsValid)
            {
                _context.Listings.Add(listing);
                _context.SaveChanges();

                // After saving, go back to browse page
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, re-show form with entered data
            return View(listing);
        }
    }
}
