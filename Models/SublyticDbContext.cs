using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Sublytic.Models
{
    public class SublyticDbContext : IdentityDbContext
    {
        public SublyticDbContext(
            DbContextOptions<SublyticDbContext> options)
            : base(options)
        {
        }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}
