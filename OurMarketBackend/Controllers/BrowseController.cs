using Microsoft.AspNetCore.Mvc;
using OurMarketBackend.Data;
using OurMarketBackend.Models; 

namespace OurMarketBackend.Controllers
{
    public class BrowseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BrowseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Browse or /Browse/Index
        public IActionResult Index()
        {
            var listings = _context.Listings.ToList();
            return View(listings);
        }

        // GET: /Browse/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Browse/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Listing listing)
        {
            if (ModelState.IsValid)
            {
                _context.Listings.Add(listing);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(listing);
        }
    }
}
