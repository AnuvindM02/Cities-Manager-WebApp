using CitiesManager.Core.Entities;
using CitiesManager.Core.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.Infrastructure.DatabaseContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        { 
        }

        public ApplicationDbContext()
        {
        }

        public virtual DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<City>().HasData(new City() { CityID = Guid.Parse("2C32DC29-9D57-4CD1-9B91-902F88CEF856"), CityName="Kochi" },
                new City() { CityID = Guid.Parse("33231AA4-2A15-4FB4-9970-6F8A0E2D923C"), CityName = "Bangalore" });

        }

    }
}
