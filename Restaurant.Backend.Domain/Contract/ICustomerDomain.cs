using System;
using System.Threading.Tasks;
using Restaurant.Backend.Dto.Account;
using Restaurant.Backend.Entities.Entities;

namespace Restaurant.Backend.Domain.Contract
{
    public interface ICustomerDomain : IDomainBase<Customer>
    {
        Task<Customer> Login(string email, string password);
        Task<bool> VerifyEmailConfirmation(Guid id);
        Task<bool> SendPhoneConfirmation(CustomerConfirmPhoneDto customerConfirmPhoneDto);
        Task<bool> VerifyPhoneConfirmation(CustomerConfirmPhoneDto customerConfirmPhoneDto);
    }
}