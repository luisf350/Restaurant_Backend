using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Restaurant.Backend.Entities.Context;
using Restaurant.Backend.Entities.Entities;
using Restaurant.Backend.Repositories.Repositories;

namespace Restaurant.Backend.RepositoriesTest
{
    public class BaseRepositoryTest
    {
        protected AppDbContext Context;
        protected CustomerRepository CustomerRepository;
        protected ConfirmCustomerRepository ConfirmCustomerRepository;
        protected CountryRepository CountryRepository;
        protected IdentificationTypeRepository IdentificationTypeRepository;

        protected Mock<ILogger<CustomerRepository>> CustomerRepositoryLogger;
        protected Mock<ILogger<CountryRepository>> CountryRepositoryLogger;
        protected Mock<ILogger<ConfirmCustomerRepository>> ConfirmCustomerRepositoryLogger;
        protected Mock<ILogger<IdentificationTypeRepository>> IdentificationTypeRepositoryLogger;

        [SetUp]
        public void Setup()
        {
            CustomerRepositoryLogger = new Mock<ILogger<CustomerRepository>>();
            CountryRepositoryLogger = new Mock<ILogger<CountryRepository>>();
            ConfirmCustomerRepositoryLogger = new Mock<ILogger<ConfirmCustomerRepository>>();
            IdentificationTypeRepositoryLogger = new Mock<ILogger<IdentificationTypeRepository>>();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Test")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            Context = new AppDbContext(options);

            CustomerRepository = new CustomerRepository(Context, CustomerRepositoryLogger.Object);
            ConfirmCustomerRepository = new ConfirmCustomerRepository(Context, ConfirmCustomerRepositoryLogger.Object);
            CountryRepository = new CountryRepository(Context, CountryRepositoryLogger.Object);
            IdentificationTypeRepository = new IdentificationTypeRepository(Context, IdentificationTypeRepositoryLogger.Object);
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var item in ConfirmCustomerRepository.GetAll().Result)
            {
                _ = ConfirmCustomerRepository.Delete(item);
            }

            foreach (var item in CustomerRepository.GetAll().Result)
            {
                _ = CustomerRepository.Delete(item);
            }

            foreach (var item in CountryRepository.GetAll().Result)
            {
                _ = CountryRepository.Delete(item);
            }

            foreach (var item in IdentificationTypeRepository.GetAll().Result)
            {
                _ = IdentificationTypeRepository.Delete(item);
            }
        }

    }
}