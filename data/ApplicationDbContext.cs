using Microsoft.EntityFrameworkCore;
using OurMarketBackend.Models;

namespace OurMarketBackend.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using OurMarketBackend.models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Listing> Listings { get; set; }

        public DbSet<ListingImage> ListingImages => Set<ListingImage>();
        public DbSet<Message> Messages { get; set; }

    }
}
