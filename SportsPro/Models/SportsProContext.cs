using Microsoft.EntityFrameworkCore;
using SportsPro.Data.Configurations;
using SportsPro.Data.SeedData;

namespace SportsPro.Models
{
    public class SportsProContext : DbContext
    {
        public SportsProContext(DbContextOptions<SportsProContext> options)
            : base(options)
        { }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Technician> Technicians { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Incident> Incidents { get; set; } = null!;
        public DbSet<Registration> Registrations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            RegistrationConfiguration.Configure(modelBuilder);

            ProductSeedData.Seed(modelBuilder);
            TechnicianSeedData.Seed(modelBuilder);
            CustomerSeedData.Seed(modelBuilder);
            IncidentSeedData.Seed(modelBuilder);
            CountrySeedData.Seed(modelBuilder);
        }
    }
}