using System;
using System.Threading.Tasks;
using Restaurant.Backend.Entities.Entities;
using Restaurant.Backend.Repositories.Infrastructure;

namespace Restaurant.Backend.Repositories.Repositories
{
    public interface IConfirmCustomerRepository : IGenericRepository<ConfirmCustomer>
    {
        Task<bool> ConfirmEmailValidation(Guid customerId);
        Task<bool> ConfirmPhoneValidation(Guid customerId);
    }
}
