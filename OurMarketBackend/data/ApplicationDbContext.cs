using Microsoft.EntityFrameworkCore;
using OurMarketBackend.models; 
namespace OurMarketBackend.data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Listing> Listings { get; set; }
    }
}
