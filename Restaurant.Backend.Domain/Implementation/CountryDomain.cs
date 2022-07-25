using Microsoft.Extensions.Configuration;
using Restaurant.Backend.Domain.Contract;
using Restaurant.Backend.Dto.Entities;
using Restaurant.Backend.Entities.Entities;
using Restaurant.Backend.Repositories.Repositories;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Backend.Domain.Implementation
{
    public class CountryDomain : DomainBase<Country>, ICountryDomain
    {
        private readonly IConfiguration _config;

        public CountryDomain(ICountryRepository repository, IConfiguration config) : base(repository)
        {
            _config = config;
        }

        public async Task<IList<Country>> GetAll()
        {
            return (await Repository.GetAll()).ToList();
        }

        public async Task<IList<CountryDto>> PullCountryList()
        {
            var client = new RestClient(_config.GetSection("AppSettings:RestCountries:Url").Value);
            var request = new RestRequest("all", DataFormat.Json);
            return await client.GetAsync<IList<CountryDto>>(request);
        }

        public async Task<int> Create(List<Country> countryList)
        {
            return await Repository.CreateBulk(countryList);
        }
    }
}
