using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Yarmoolka.Models
{
    public class YarmoolkaContext : IdentityDbContext<ApplicationUser>
    {
        public YarmoolkaContext(DbContextOptions<YarmoolkaContext> options)
            : base(options)
        {
        }

        public DbSet<YarmoolkaClass> Yarmoolka { get; set; }

        public DbSet<StoreBranch> StoreBranch { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Customer> Customer { get; set; }

        public DbSet<Supplier> Supplier { get; set; }
    }
}
