using Restaurant.Backend.Domain.Contract;
using Restaurant.Backend.Entities.Entities;
using Restaurant.Backend.Repositories.Repositories;

namespace Restaurant.Backend.Domain.Implementation
{
    public class ConfirmCustomerDomain : DomainBase<ConfirmCustomer>, IConfirmCustomerDomain
    {
        public ConfirmCustomerDomain(IConfirmCustomerRepository repository) : base(repository)
        {
        }
    }
}
