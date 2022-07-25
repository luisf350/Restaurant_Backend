using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Restaurant.Backend.CommonApi.Utils;
using Restaurant.Backend.Dto.Account;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Restaurant.Backend.CommonApi.Base;

namespace Restaurant.Backend.Account.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly IConfiguration _config;

        public CompanyController(ILogger<CompanyController> logger, IConfiguration config, IMapper mapper) 
            : base(logger, mapper)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Task.FromResult(Ok("Working from Company Controller"));
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(CompanyLoginDto login)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, $"{Guid.NewGuid()}")
            };

            return Ok(new
            {
                token = JwtCreationUtil.CreateJwtToken(claims, _config)
            });
        }
    }
}
