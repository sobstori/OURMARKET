using Microsoft.EntityFrameworkCore;
using OURMARKET.Models;

namespace OURMARKET.Data
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