using Restaurant.Backend.Common.Constants;
using Restaurant.Backend.Common.Utils;
using Restaurant.Backend.Domain.Contract;
using Restaurant.Backend.Entities.Entities;
using Restaurant.Backend.Repositories.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Restaurant.Backend.Common.Notifications;
using Restaurant.Backend.Dto.Account;

namespace Restaurant.Backend.Domain.Implementation
{
    public class CustomerDomain : DomainBase<Customer>, ICustomerDomain
    {
        private readonly ILogger<ICustomerDomain> _logger;
        private readonly IConfirmCustomerRepository _confirmCustomerRepository;
        private readonly INexmoNotifications _nexmoNotifications;

        public CustomerDomain(ICustomerRepository repository, ILogger<ICustomerDomain> logger, IConfirmCustomerRepository confirmCustomerRepository, 
            INexmoNotifications nexmoNotifications)
            : base(repository)
        {
            _logger = logger;
            _confirmCustomerRepository = confirmCustomerRepository;
            _nexmoNotifications = nexmoNotifications;
        }

        public async Task<Customer> Login(string email, string password)
        {
            var customer = (await Repository.GetAll(x => x.Email == email)).FirstOrDefault();

            if (customer == null || !PasswordUtils.VerifyPasswordHash(password, customer.PasswordHash, customer.PasswordSalt))
            {
                throw new Exception(Constants.LoginNotValid);
            }
            if (!customer.Active)
            {
                throw new Exception(Constants.CustomerNotActive);
            }

            return customer;
        }

        public async Task<bool> VerifyEmailConfirmation(Guid id)
        {
            return await _confirmCustomerRepository.ConfirmEmailValidation(id);
        }

        public async Task<bool> SendPhoneConfirmation(CustomerConfirmPhoneDto customerConfirmPhoneDto)
        {
            var confirmCustomer = await _confirmCustomerRepository.FirstOfDefaultAsync(x =>
                x.CustomerId == customerConfirmPhoneDto.CustomerId);

            if (confirmCustomer == null)
            {
                return false;
            }

            var verifyResponse = _nexmoNotifications.SendNotification(customerConfirmPhoneDto.PhoneNumber);

            if (!string.IsNullOrEmpty(verifyResponse.error_text))
            {
                _logger.LogError(string.Format(Constants.ErrorFromNexmo, customerConfirmPhoneDto.CustomerId,
                    customerConfirmPhoneDto.PhoneNumber, verifyResponse.error_text));
                return false;
            }

            confirmCustomer.UniquePhoneKey = verifyResponse.request_id;
            return await _confirmCustomerRepository.Update(confirmCustomer);
        }

        public async Task<bool> VerifyPhoneConfirmation(CustomerConfirmPhoneDto customerConfirmPhoneDto)
        {
            if (string.IsNullOrEmpty(customerConfirmPhoneDto.UniquePhoneKey))
            {
                throw new Exception(Constants.MissingNexmoKey);
            }

            var verifyResponse = _nexmoNotifications.VerifyNotification(customerConfirmPhoneDto.UniquePhoneKey, customerConfirmPhoneDto.Code);

            if (!string.IsNullOrEmpty(verifyResponse.error_text))
            {
                _logger.LogError(string.Format(Constants.ErrorFromNexmo, customerConfirmPhoneDto.CustomerId,
                    customerConfirmPhoneDto.PhoneNumber, verifyResponse.error_text));
                return false;
            }

            var confirmCustomer = await _confirmCustomerRepository.FirstOfDefaultAsync(
                x => x.CustomerId == customerConfirmPhoneDto.CustomerId, x => x.Customer);

            if (confirmCustomer == null)
            {
                throw new Exception(string.Format(Constants.NotFound, $"Confirm Customer CustomerId={customerConfirmPhoneDto.CustomerId}."));
            }

            confirmCustomer.Customer.VerifiedPhoneNumber = true;
            return await _confirmCustomerRepository.Update(confirmCustomer);

        }
    }
}
