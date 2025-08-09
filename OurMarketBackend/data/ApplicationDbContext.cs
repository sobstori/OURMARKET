using Microsoft.EntityFrameworkCore;
using OurMarketBackend.Models;

namespace OurMarketBackend.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Listing> Listings { get; set; }
        public DbSet<Message> Messages { get; set; }

    }
}
