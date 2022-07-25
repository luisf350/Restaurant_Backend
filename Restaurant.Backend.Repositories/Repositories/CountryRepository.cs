using Microsoft.Extensions.Logging;
using Restaurant.Backend.Entities.Context;
using Restaurant.Backend.Entities.Entities;
using Restaurant.Backend.Repositories.Infrastructure;

namespace Restaurant.Backend.Repositories.Repositories
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        public CountryRepository(AppDbContext context, ILogger<CountryRepository> logger) : base(context, logger)
        {
        }
    }
}
