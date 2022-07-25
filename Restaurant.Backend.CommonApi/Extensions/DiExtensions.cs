using Microsoft.Extensions.DependencyInjection;
using Restaurant.Backend.Common.Enums;
using Restaurant.Backend.Common.Notifications;
using Restaurant.Backend.Domain.Contract;
using Restaurant.Backend.Domain.Implementation;
using Restaurant.Backend.Repositories.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace Restaurant.Backend.CommonApi.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class DiExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, Microservice service)
        {
            services.AddScoped<INexmoNotifications, NexmoNotifications>();

            switch (service)
            {
                case Microservice.Account:
                    services.AddScoped<ICountryDomain, CountryDomain>();
                    services.AddScoped<ICustomerDomain, CustomerDomain>();
                    services.AddScoped<IConfirmCustomerDomain, ConfirmCustomerDomain>();
                    services.AddScoped<IIdentificationTypeDomain, IdentificationTypeDomain>();

                    services.AddScoped<ICountryRepository, CountryRepository>();
                    services.AddScoped<ICustomerRepository, CustomerRepository>();
                    services.AddScoped<IConfirmCustomerRepository, ConfirmCustomerRepository>();
                    services.AddScoped<IIdentificationTypeRepository, IdentificationTypeRepository>();
                    break;
            }
        }
    }
}
