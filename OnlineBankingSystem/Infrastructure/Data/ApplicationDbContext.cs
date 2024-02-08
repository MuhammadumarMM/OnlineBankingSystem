using Domain.Entities.Entity;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Card > cards  { get; set; }
        public DbSet<Credit> credits { get; set; }
        public DbSet<Deposit> deposits { get; set; }

        public DbSet<Trancation> trancations { get; set; }
        public DbSet<BankCard> bankcards { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Your configurations here

            base.OnModelCreating(builder);
        }
    }
}