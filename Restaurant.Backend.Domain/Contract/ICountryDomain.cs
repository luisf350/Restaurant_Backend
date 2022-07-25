using Restaurant.Backend.Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Restaurant.Backend.Dto.Entities;

namespace Restaurant.Backend.Domain.Contract
{
    public interface ICountryDomain : IDomainBase<Country>
    {
        Task<IList<Country>> GetAll();
        Task<IList<CountryDto>> PullCountryList();
        Task<int> Create(List<Country> countryList);
    }
}