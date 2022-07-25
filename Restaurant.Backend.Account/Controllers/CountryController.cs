using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurant.Backend.CommonApi.Base;
using Restaurant.Backend.Domain.Contract;
using Restaurant.Backend.Dto.Entities;
using Restaurant.Backend.Entities.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Backend.Account.Controllers
{
    [AllowAnonymous]
    public class CountryController : BaseController
    {
        private readonly ICountryDomain _countryDomain;

        public CountryController(ILogger<CountryController> logger, IMapper mapper, ICountryDomain countryDomain) : base(logger, mapper)
        {
            _countryDomain = countryDomain;
        }

        [ResponseCache(Duration = 1440, Location = ResponseCacheLocation.Any)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var resultDb = await _countryDomain.GetAll();

            if (resultDb.Any())
            {
                return Ok(Mapper.Map<IList<CountryDto>>(resultDb));
            }

            var resultService = await _countryDomain.PullCountryList();
            var countryList = Mapper.Map<List<Country>>(resultService);

            countryList.ForEach(country =>
            {
                var valor = resultService?.FirstOrDefault(x => x.Name == country.Name)?.CallingCodes?.FirstOrDefault();
                if (!string.IsNullOrEmpty(valor))
                {
                    country.CallingCode = int.Parse(valor.Split(" ")[0]);
                }
            });

            await _countryDomain.Create(countryList);
            resultDb = await _countryDomain.GetAll();

            return Ok(Mapper.Map<IList<CountryDto>>(resultDb));
        }
    }
}
