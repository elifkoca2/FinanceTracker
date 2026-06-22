using FinanceTracker.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Data
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    { 
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<WatchlistItem> WatchlistItems { get; set; }
        public DbSet<PriceAlert> PriceAlerts { get; set; }

      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WatchlistItem>()
                .Property(w => w.AlertDirection)
                .HasConversion<string>();

            // modelBuilder.Entity<WatchlistItem>()
            //     .Property(w => w.AlertDirection)
            //     .HasConversion(
            //         v => v.ToString(),                              
            //         v => Enum.Parse<AlertDirection>(v)             
            //     );
            //

        }
    }
}
