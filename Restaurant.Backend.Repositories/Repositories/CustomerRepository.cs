using Microsoft.Extensions.Logging;
using Restaurant.Backend.Entities.Context;
using Restaurant.Backend.Entities.Entities;
using Restaurant.Backend.Repositories.Infrastructure;

namespace Restaurant.Backend.Repositories.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context, ILogger<CustomerRepository> logger) : base(context, logger)
        {
        }
    }
}
