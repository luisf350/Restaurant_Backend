using Microsoft.EntityFrameworkCore;
using Restaurant.Backend.Entities.Entities;

namespace Restaurant.Backend.Entities.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<IdentificationType> IdentificationTypes { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<ConfirmCustomer> ConfirmCustomers { get; set; }

        public DbSet<Country> Countries { get; set; }
    }
}
