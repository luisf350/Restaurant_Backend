using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Restaurant.Backend.Common.Constants;
using Restaurant.Backend.Common.Utils;
using Restaurant.Backend.CommonApi.Base;
using Restaurant.Backend.CommonApi.Utils;
using Restaurant.Backend.Domain.Contract;
using Restaurant.Backend.Dto.Account;
using Restaurant.Backend.Dto.Entities;
using Restaurant.Backend.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Restaurant.Backend.Account.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly ICustomerDomain _customerDomain;
        private readonly IConfirmCustomerDomain _confirmCustomerDomain;

        public CustomerController(ILogger<CustomerController> logger, IConfiguration config, IMapper mapper, ICustomerDomain customerDomain, IConfirmCustomerDomain confirmCustomerDomain)
            : base(logger, mapper)
        {
            _config = config;
            _customerDomain = customerDomain;
            _confirmCustomerDomain = confirmCustomerDomain;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var resultTypes = await _customerDomain.GetAll(null, null, x => x.IdentificationType);

            return Ok(Mapper.Map<IList<CustomerDto>>(resultTypes));
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var resultType = await _customerDomain.FirstOfDefaultAsync(x => x.Id == id, x => x.IdentificationType);

            return resultType == null ?
                (IActionResult)NotFound(string.Format(Constants.NotFound, id))
                : Ok(Mapper.Map<CustomerDto>(resultType));
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(CustomerLoginDto login)
        {
            var userFromRepo = await _customerDomain.Login(login.Email, login.Password);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, $"{userFromRepo.Id}"),
                new Claim(ClaimTypes.Name, $"{userFromRepo.FirstName} {userFromRepo.LastName}")
            };

            return Ok(new
            {
                token = await JwtCreationUtil.CreateJwtToken(claims, _config)
            });
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CustomerDto customerDto)
        {
            var dbCustomer = await _customerDomain.FirstOfDefaultAsync(x =>
                x.Email.ToUpper() == customerDto.Email.ToUpper());

            if (dbCustomer != null)
            {
                return BadRequest(Constants.EmailInUse);
            }

            var customer = Mapper.Map<Customer>(customerDto);

            PasswordUtils.CreatePasswordHash(customerDto.Password, out var passwordHash, out var passwordSalt);
            customer.PasswordHash = passwordHash;
            customer.PasswordSalt = passwordSalt;
            customer.Id = Guid.NewGuid();
            customer.Active = true;

            var result = await _customerDomain.Create(customer);

            if (result == 0)
            {
                return BadRequest(Constants.OperationNotCompleted);
            }

            Task confirmCustomerTask = _confirmCustomerDomain.Create(new ConfirmCustomer
            {
                CustomerId = customer.Id,
                UniqueEmailKey = PasswordUtils.GenerateTempKey(customer.Email),
                ExpirationEmail = DateTimeOffset.UtcNow.AddMinutes(10)
            });

            List<Task> taskList = new List<Task> { confirmCustomerTask };
            // TODO: Add logic, in Task list, to send email for confirmation

            await Task.WhenAll(taskList);

            return Ok(customerDto);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(CustomerDto modelDto)
        {
            var model = await _customerDomain.Find(modelDto.Id);
            if (model == null)
            {
                return NotFound(string.Format(Constants.NotFound, modelDto.Id));
            }

            model.IdentificationTypeId = modelDto.IdentificationTypeId;
            model.IdentificationNumber = modelDto.IdentificationNumber;
            model.FirstName = modelDto.FirstName;
            model.LastName = modelDto.LastName;
            model.Birthday = modelDto.Birthday;
            model.Gender = modelDto.Gender;

            var result = await _customerDomain.Update(model);

            return result ?
                Ok(Mapper.Map<CustomerDto>(model))
                : (IActionResult)BadRequest(Constants.OperationNotCompleted);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(CustomerDto modelDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Constants.ModelNotValid);
            }

            var model = await _customerDomain.Find(modelDto.Id);
            if (model == null)
            {
                return NotFound(string.Format(Constants.NotFound, modelDto.Id));
            }

            var result = await _customerDomain.Delete(model);

            return result == 1 ?
                Ok(Mapper.Map<CustomerDto>(model))
                : (IActionResult)BadRequest(Constants.OperationNotCompleted);
        }

        [AllowAnonymous]
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(CustomerConfirmEmailDto confirmEmailDto)
        {
            var customerCustomer =
                await _confirmCustomerDomain.FirstOfDefaultAsync(x => x.Customer.Email == confirmEmailDto.Email, x => x.Customer);

            if (customerCustomer == null)
            {
                return NotFound(string.Format(Constants.NotFound, confirmEmailDto.Email));
            }

            if (!customerCustomer.UniqueEmailKey.Equals(confirmEmailDto.EmailKey, StringComparison.InvariantCulture) ||
                DateTimeOffset.UtcNow > customerCustomer.ExpirationEmail)
            {
                return NotFound(Constants.SendNewKeyForActivation);
            }

            return await _customerDomain.VerifyEmailConfirmation(customerCustomer.CustomerId) ?
                Ok(confirmEmailDto)
                : (IActionResult)BadRequest(Constants.OperationNotCompleted);
        }

        [HttpPost("ConfirmPhone")]
        public async Task<IActionResult> ConfirmPhone(CustomerConfirmPhoneDto confirmPhoneDto)
        {
            return await _customerDomain.SendPhoneConfirmation(confirmPhoneDto)
                ? Ok()
                : (IActionResult)BadRequest(Constants.OperationNotCompleted);
        }

        [HttpPost("VerifyConfirmPhone")]
        public async Task<IActionResult> VerifyConfirmPhone(CustomerConfirmPhoneDto confirmPhoneDto)
        {
            return await _customerDomain.VerifyPhoneConfirmation(confirmPhoneDto)
                ? Ok()
                : (IActionResult)BadRequest(Constants.OperationNotCompleted);
        }
    }
}
