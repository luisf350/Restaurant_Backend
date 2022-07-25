using Microsoft.Extensions.Logging;
using Restaurant.Backend.Entities.Context;
using Restaurant.Backend.Entities.Entities;
using Restaurant.Backend.Repositories.Infrastructure;

namespace Restaurant.Backend.Repositories.Repositories
{
    public class IdentificationTypeRepository : GenericRepository<IdentificationType>, IIdentificationTypeRepository
    {
        public IdentificationTypeRepository(AppDbContext context, ILogger<IdentificationTypeRepository> logger) : base(context, logger)
        {
        }
    }
}
