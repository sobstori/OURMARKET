using Microsoft.AspNetCore.Mvc;
using OURMARKET.Models;
using System.Collections.Generic;

namespace OURMARKET.Controllers
{
    public class BrowseController : Controller
    {
        public IActionResult Index()
        {
            var listings = new List<Listing>
            {
                new Listing { ImageUrl = "/images/phone.jpg", Title = "iPhone 13 - Like New", Price = "$650", Location = "Los Angeles" },
                new Listing { ImageUrl = "/images/couch.jpg", Title = "Vintage Couch", Price = "$120", Location = "Chicago" },
                new Listing { ImageUrl = "/images/coffee.jpg", Title = "Part-Time Barista Job", Price = "$15/hr", Location = "New York" }
            };
            return View(listings);
        }
    }
}
