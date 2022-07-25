using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Restaurant.Backend.Common.Notifications;
using Restaurant.Backend.CommonApi.Base;
using Restaurant.Backend.CommonApi.Profiles;
using Restaurant.Backend.Domain.Contract;
using Restaurant.Backend.Domain.Implementation;
using Restaurant.Backend.Entities.Context;
using Restaurant.Backend.Repositories.Repositories;
using RestSharp;

namespace Restaurant.Backend.Account.Test
{
    public class BaseControllerTest<TController>
        where TController : BaseController
    {
        protected ICountryRepository CountryRepository;
        protected ICustomerRepository CustomerRepository;
        protected IConfirmCustomerRepository ConfirmCustomerRepository;
        protected IIdentificationTypeRepository IdentificationTypeRepository;
        protected Mock<IConfiguration> Config;
        protected ICountryDomain CountryDomain;
        protected ICustomerDomain CustomerDomain;
        protected IConfirmCustomerDomain ConfirmCustomerDomain;
        protected IIdentificationTypeDomain IdentificationTypeDomain;
        protected IMapper Mapper;
        protected AppDbContext Context;

        protected Mock<ILogger<TController>> LoggerController;
        protected Mock<INexmoNotifications> NexmoNotifications;

        protected Mock<ILogger<CountryRepository>> CountryRepositoryLogger;
        protected Mock<ILogger<CustomerRepository>> CustomerRepositoryLogger;
        protected Mock<ILogger<ConfirmCustomerRepository>> ConfirmCustomerRepositoryLogger;
        protected Mock<ILogger<IdentificationTypeRepository>> IdentificationTypeRepositoryLogger;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Test")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            Context = new AppDbContext(options);

            LoggerController = new Mock<ILogger<TController>>();
            NexmoNotifications = new Mock<INexmoNotifications>();
            Config = new Mock<IConfiguration>(); 
            Config.Setup(x => x.GetSection("AppSettings:Token").Value).Returns("SuperSecretKey2020");
            Config.Setup(x => x.GetSection("AppSettings:RestCountries:Url").Value).Returns("https://restcountries.eu/rest/v2");

            // Logger for Repositories
            CountryRepositoryLogger = new Mock<ILogger<CountryRepository>>();
            CustomerRepositoryLogger = new Mock<ILogger<CustomerRepository>>();
            ConfirmCustomerRepositoryLogger = new Mock<ILogger<ConfirmCustomerRepository>>();
            IdentificationTypeRepositoryLogger = new Mock<ILogger<IdentificationTypeRepository>>();

            // Repositories
            CountryRepository = new CountryRepository(Context, CountryRepositoryLogger.Object);
            CustomerRepository = new CustomerRepository(Context, CustomerRepositoryLogger.Object);
            ConfirmCustomerRepository = new ConfirmCustomerRepository(Context, ConfirmCustomerRepositoryLogger.Object);
            IdentificationTypeRepository = new IdentificationTypeRepository(Context, IdentificationTypeRepositoryLogger.Object);

            // Domains
            CountryDomain = new CountryDomain(CountryRepository, Config.Object);
            CustomerDomain = new CustomerDomain(CustomerRepository, null, ConfirmCustomerRepository, NexmoNotifications.Object);
            ConfirmCustomerDomain = new ConfirmCustomerDomain(ConfirmCustomerRepository);
            IdentificationTypeDomain = new IdentificationTypeDomain(IdentificationTypeRepository);

            // AutoMapper
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var item in ConfirmCustomerRepository.GetAll().Result)
            {
                ConfirmCustomerRepository.Delete(item);
            }

            foreach (var item in CustomerRepository.GetAll().Result)
            {
                CustomerRepository.Delete(item);
            }

            foreach (var item in IdentificationTypeRepository.GetAll().Result)
            {
                IdentificationTypeRepository.Delete(item);
            }
        }
    }
}
