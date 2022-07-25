using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurant.Backend.Entities.Context;
using Restaurant.Backend.Entities.Entities;
using Restaurant.Backend.Repositories.Infrastructure;

namespace Restaurant.Backend.Repositories.Repositories
{
    public class ConfirmCustomerRepository : GenericRepository<ConfirmCustomer>, IConfirmCustomerRepository
    {
        public ConfirmCustomerRepository(AppDbContext context, ILogger<ConfirmCustomerRepository> logger) : base(context, logger)
        {
        }

        public async Task<bool> ConfirmEmailValidation(Guid customerId)
        {
            var customer = (await Context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == customerId));
            if (customer == null)
            {
                return false;
            }

            customer.VerifiedEmail = true;
            Context.Customers.Update(customer);
            return true;
        }

        public async Task<bool> ConfirmPhoneValidation(Guid customerId)
        {
            var customer = (await Context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == customerId));
            if (customer == null)
            {
                return false;
            }

            customer.VerifiedPhoneNumber = true;
            Context.Customers.Update(customer);
            return true;
        }
    }
}
