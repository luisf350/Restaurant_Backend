using Microsoft.Extensions.Logging;
using Restaurant.Backend.Domain.Contract;
using Restaurant.Backend.Entities.Entities;
using Restaurant.Backend.Repositories.Repositories;

namespace Restaurant.Backend.Domain.Implementation
{
    public class IdentificationTypeDomain : DomainBase<IdentificationType>, IIdentificationTypeDomain
    {
        public IdentificationTypeDomain(IIdentificationTypeRepository repository) : base(repository)
        {
        }
    }
}
