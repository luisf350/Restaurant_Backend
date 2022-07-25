using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Restaurant.Backend.Account.Controllers;
using Restaurant.Backend.Common.Constants;
using Restaurant.Backend.Dto.Account;
using Restaurant.Backend.Dto.Entities;
using Restaurant.Backend.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Backend.Account.Test
{
    public class CustomerControllerTest : BaseControllerTest<CustomerController>
    {
        [Test]
        public void GetAllTest()
        {
            // Setup
            GenerateDbRecords(10);
            var controller = new CustomerController(LoggerController.Object, Config.Object, Mapper, CustomerDomain, ConfirmCustomerDomain);

            // Act
            var result = controller.GetAll().Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(typeof(OkObjectResult), result.GetType());
            Assert.AreSame(typeof(List<CustomerDto>), (result as OkObjectResult)?.Value.GetType());
            Assert.AreEqual(10, ((result as OkObjectResult)?.Value as List<CustomerDto>)?.Count);
        }

        [Test]
        public void GetTest()
        {
            // Setup
            var id = Guid.NewGuid();
            GenerateDbRecord(id, "test@mail.com");

            var controller = new CustomerController(LoggerController.Object, Config.Object, Mapper, CustomerDomain, ConfirmCustomerDomain);

            // Act
            var result = controller.Get(id).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(typeof(OkObjectResult), result.GetType());
            Assert.AreSame(typeof(CustomerDto), (result as OkObjectResult)?.Value.GetType());
        }

        [Test]
        public void LoginTest()
        {
            // Setup
            var id = Guid.NewGuid();
            GenerateDbRecord(id, "test@mail.com");
            var model = new CustomerLoginDto
            {
                Email = "test@mail.com",
                Password = "Password1"
            };

            var controller = new CustomerController(LoggerController.Object, Config.Object, Mapper, CustomerDomain, ConfirmCustomerDomain);

            // Act
            var expectedException = Task.FromResult(controller.Login(model)).Result;

            // Assert
            Assert.AreSame(Constants.LoginNotValid, expectedException.Exception?.InnerException?.Message);
        }

        [Test]
        public void CreateTest()
        {
            // Setup
            var id = Guid.NewGuid();
            var modelDto = new CustomerDto
            {
                Id = id,
                FirstName = $"First Name for {id}",
                LastName = $"Last Name for {id}",
                Birthday = new DateTime(2020, 01, 01),
                Email = $"{id}@mail.com",
                Password = "Password",
                Gender = (short)new Random().Next(1, 2),
                IdentificationNumber = new Random().Next(8000000, 1100000000),
                PhoneNumber = new Random().Next(300000000, 326000000).ToString(),
            };

            var controller = new CustomerController(LoggerController.Object, Config.Object, Mapper, CustomerDomain, ConfirmCustomerDomain);

            // Act
            var result = controller.Create(modelDto).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(typeof(OkObjectResult), result.GetType());
            Assert.AreSame(typeof(CustomerDto), (result as OkObjectResult)?.Value.GetType());
        }

        [Test]
        public void CreateDuplicateEmailTest()
        {
            // Setup
            var id = Guid.NewGuid();
            var modelDto = new CustomerDto
            {
                Id = id,
                FirstName = $"First Name for {id}",
                LastName = $"Last Name for {id}",
                Birthday = new DateTime(2020, 01, 01),
                Email = "email@mail.com",
                Password = "Password",
                Gender = (short)new Random().Next(1, 2),
                IdentificationNumber = new Random().Next(8000000, 1100000000),
                PhoneNumber = new Random().Next(300000000, 326000000).ToString(),
            };

            var controller = new CustomerController(LoggerController.Object, Config.Object, Mapper, CustomerDomain, ConfirmCustomerDomain);

            // Act
            var result = controller.Create(modelDto).Result;
            var result2 = controller.Create(modelDto).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(typeof(OkObjectResult), result.GetType());
            Assert.AreSame(typeof(CustomerDto), (result as OkObjectResult)?.Value.GetType());

            Assert.IsNotNull(result2);
            Assert.AreSame(typeof(BadRequestObjectResult), result2.GetType());
            Assert.AreEqual(Constants.EmailInUse, (result2 as BadRequestObjectResult)?.Value);
        }

        [Test]
        public void UpdateNotFoundTest()
        {
            // Setup
            var modelDto = new CustomerDto
            {
                Id = Guid.NewGuid(),
            };

            var controller = new CustomerController(LoggerController.Object, Config.Object, Mapper, CustomerDomain, ConfirmCustomerDomain);

            // Act
            var result = controller.Update(modelDto).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(typeof(NotFoundObjectResult), result.GetType());
        }

        [Test]
        public void UpdateTest()
        {
            // Setup
            var id = Guid.NewGuid();
            GenerateDbRecord(id, "test@mail.com");
            var newIdentificationType = new IdentificationType
            {
                Id = Guid.NewGuid(),
                Name = "New Identification Type",
                Description = "Description for new Identification Type"
            };
            IdentificationTypeRepository.Create(newIdentificationType);

            var modelDto = new CustomerDto
            {
                Id = id,
                IdentificationTypeId = newIdentificationType.Id,
                IdentificationNumber = 1234567890,
                FirstName = "Updated FirstName",
                LastName = "Updated LastName",
                Birthday = DateTime.MinValue,
                Gender = 1,
                PhoneNumber = "01800-NotChange",
                Email = "NotChange@mail.com",
                Password = "NotChange"
            };

            var controller = new CustomerController(LoggerController.Object, Config.Object, Mapper, CustomerDomain, ConfirmCustomerDomain);

            // Act
            var result = controller.Update(modelDto).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(typeof(OkObjectResult), result.GetType());
            Assert.AreSame(typeof(CustomerDto), (result as OkObjectResult)?.Value.GetType());
            Assert.AreEqual(modelDto.IdentificationTypeId, ((result as OkObjectResult)?.Value as CustomerDto)?.IdentificationTypeId);
            Assert.AreEqual(modelDto.IdentificationNumber, ((result as OkObjectResult)?.Value as CustomerDto)?.IdentificationNumber);
            Assert.AreEqual(modelDto.FirstName, ((result as OkObjectResult)?.Value as CustomerDto)?.FirstName);
            Assert.AreEqual(modelDto.LastName, ((result as OkObjectResult)?.Value as CustomerDto)?.LastName);
            Assert.AreEqual(modelDto.Birthday, ((result as OkObjectResult)?.Value as CustomerDto)?.Birthday);
            Assert.AreNotEqual(modelDto.PhoneNumber, ((result as OkObjectResult)?.Value as CustomerDto)?.PhoneNumber);
            Assert.AreNotEqual(modelDto.Email, ((result as OkObjectResult)?.Value as CustomerDto)?.Email);
            Assert.AreNotEqual(modelDto.Password, ((result as OkObjectResult)?.Value as CustomerDto)?.Password);
        }

        [Test]
        public void DeleteNotFoundTest()
        {
            // Setup
            var modelDto = new CustomerDto
            {
                Id = Guid.NewGuid(),
            };

            var controller = new CustomerController(LoggerController.Object, Config.Object, Mapper, CustomerDomain, ConfirmCustomerDomain);

            // Act
            var result = controller.Delete(modelDto).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(typeof(NotFoundObjectResult), result.GetType());
        }

        [Test]
        public void DeleteTest()
        {
            // Setup
            var id = Guid.NewGuid();
            GenerateDbRecord(id, "test@mail.com");
            var modelDto = new CustomerDto
            {
                Id = id,
            };

            var controller = new CustomerController(LoggerController.Object, Config.Object, Mapper, CustomerDomain, ConfirmCustomerDomain);

            // Act
            var result = controller.Delete(modelDto).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(typeof(OkObjectResult), result.GetType());
        }

        #region Private methods

        private void GenerateDbRecords(int numberRecords)
        {
            var identificationType = new IdentificationType
            {
                Id = Guid.NewGuid(),
                Name = "Identification Type test name",
                Description = "Identification Type test description",
                CreationDate = DateTimeOffset.UtcNow.AddDays(1).AddHours(2).AddMinutes(3),
                ModificationDate = DateTimeOffset.MinValue
            };
            IdentificationTypeRepository.Create(identificationType);

            for (int i = 0; i < numberRecords; i++)
            {
                var id = Guid.NewGuid();
                CustomerRepository.Create(new Customer
                {
                    Id = id,
                    IdentificationTypeId = identificationType.Id,
                    IdentificationType = identificationType,
                    FirstName = $"First Name for {id}",
                    LastName = $"Last Name for {id}",
                    Birthday = new DateTime(2020, 01, 01),
                    CreationDate = DateTimeOffset.UtcNow.AddDays(1).AddHours(2).AddMinutes(3),
                    ModificationDate = DateTimeOffset.MinValue,
                    Email = $"email{i}@mail.com",
                    Gender = (short)new Random().Next(1, 2),
                    IdentificationNumber = new Random().Next(8000000, 1100000000),
                    PasswordHash = Encoding.ASCII.GetBytes("PasswordHash"),
                    PasswordSalt = Encoding.ASCII.GetBytes("PasswordSalt"),
                    PhoneNumber = new Random().Next(300000000, 326000000).ToString(),
                    VerifiedEmail = false,
                    VerifiedPhoneNumber = false,
                    Active = true
                });
            }
        }

        private void GenerateDbRecord(Guid id, string email, string phoneNumber = null)
        {
            var identificationType = new IdentificationType
            {
                Id = id,
                Name = "Identification Type test name",
                Description = "Identification Type test description",
                CreationDate = DateTimeOffset.UtcNow.AddDays(1).AddHours(2).AddMinutes(3),
                ModificationDate = DateTimeOffset.MinValue
            };
            IdentificationTypeRepository.Create(identificationType);

            CustomerRepository.Create(new Customer
            {
                Id = id,
                IdentificationTypeId = id,
                IdentificationType = identificationType,
                FirstName = $"First Name for {id}",
                LastName = $"Last Name for {id}",
                Birthday = new DateTime(2020, 01, 01),
                CreationDate = DateTimeOffset.UtcNow.AddDays(1).AddHours(2).AddMinutes(3),
                ModificationDate = DateTimeOffset.MinValue,
                Email = email,
                Gender = (short)new Random().Next(1, 2),
                IdentificationNumber = new Random().Next(8000000, 1100000000),
                PasswordHash = Encoding.ASCII.GetBytes("PasswordHash"),
                PasswordSalt = Encoding.ASCII.GetBytes("PasswordSalt"),
                PhoneNumber = phoneNumber ?? new Random().Next(300000000, 326000000).ToString(),
                VerifiedEmail = false,
                VerifiedPhoneNumber = false,
                Active = true
            });
        }

        #endregion
    }
}
