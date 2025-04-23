using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Data.Configurations
{
    public static class RegistrationConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Registration>().HasKey(r => new { r.CustomerID, r.ProductID });
        }
    }
}
