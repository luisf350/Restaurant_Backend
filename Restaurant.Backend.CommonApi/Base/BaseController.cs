using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Restaurant.Backend.CommonApi.Base
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected readonly ILogger Logger;
        protected readonly IMapper Mapper;
        
        public BaseController(ILogger logger, IMapper mapper)
        {
            Logger = logger;
            Mapper = mapper;
        }
    }
}
