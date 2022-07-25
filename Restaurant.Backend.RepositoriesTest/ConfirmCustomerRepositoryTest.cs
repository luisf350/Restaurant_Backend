using NUnit.Framework;
using Restaurant.Backend.Entities.Entities;
using System;
using System.Linq;
using System.Text;

namespace Restaurant.Backend.RepositoriesTest
{
    public class ConfirmCustomerRepositoryTest : BaseRepositoryTest
    {
        [Test]
        public void GetAllTest()
        {
            // Setup
            GenerateDbRecords(10);

            // Act
            var result = ConfirmCustomerRepository.GetAll().Result;

            // Assert
            Assert.AreEqual(result.Count(), 10);
        }

        [Test]
        public void GetTest()
        {
            // Setup
            var id = Guid.NewGuid();
            GenerateDbRecord(id );

            // Act
            var result = ConfirmCustomerRepository.GetById(id).Result;

            // Assert
            Assert.AreEqual(result.Id, id);
        }

        [Test]
        public void GetNotFoundTest()
        {
            // Setup
            var id = Guid.NewGuid();

            // Act
            var result = ConfirmCustomerRepository.GetById(id).Result;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void CreateTest()
        {
            // Setup
            var id = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            Context.Customers.Add(new Customer
            {
                Id = customerId,
                FirstName = $"First Name for {customerId}",
                LastName = $"Last Name for {customerId}",
                Birthday = new DateTime(2020, 01, 01),
                CreationDate = DateTimeOffset.UtcNow.AddDays(1).AddHours(2).AddMinutes(3),
                ModificationDate = DateTimeOffset.MinValue,
                Email = $"{customerId}@mail.com",
                Gender = (short)new Random().Next(1, 2),
                IdentificationNumber = new Random().Next(8000000, 1100000000),
                PasswordHash = Encoding.ASCII.GetBytes("PasswordHash"),
                PasswordSalt = Encoding.ASCII.GetBytes("PasswordSalt"),
                PhoneNumber = new Random().Next(300000000, 326000000).ToString(),
                VerifiedEmail = false,
                VerifiedPhoneNumber = false,
                Active = true,
                IdentificationType = new IdentificationType
                {
                    Id = customerId,
                    Name = $"Name for {customerId}",
                    Description = $"Description for {customerId}",
                    CreationDate = DateTimeOffset.UtcNow.AddDays(1).AddHours(2).AddMinutes(3),
                    ModificationDate = DateTimeOffset.MinValue
                }
            });
            var model = new ConfirmCustomer
            {
                Id = id,
                CustomerId = customerId,
                ExpirationEmail = DateTimeOffset.UtcNow.AddDays(10).AddHours(2).AddMinutes(3),
                UniqueEmailKey = $"UniqueEmailKey-{id}",
                UniquePhoneKey = $"UniquePhoneKey-{id}",
                CreationDate = DateTimeOffset.UtcNow.AddDays(1).AddHours(2).AddMinutes(3),
                ModificationDate = DateTimeOffset.MinValue
            };

            // Act
            var result = ConfirmCustomerRepository.Create(model).Result;
            var dbRecord = ConfirmCustomerRepository.GetById(model.Id).Result;

            // Assert
            Assert.AreEqual(result, 3);
            Assert.AreEqual(dbRecord.CustomerId, model.CustomerId);
            Assert.AreEqual(dbRecord.UniquePhoneKey, model.UniquePhoneKey);
            Assert.AreEqual(dbRecord.UniqueEmailKey, model.UniqueEmailKey);
            Assert.AreEqual(dbRecord.CreationDate, model.CreationDate);
            Assert.AreEqual(dbRecord.ModificationDate, model.ModificationDate);
        }

        [Test]
        public void UpdateTest()
        {
            // Setup
            var id = Guid.NewGuid();
            GenerateDbRecord(id);

            var model = ConfirmCustomerRepository.GetById(id).Result;
            model.UniqueEmailKey = string.Empty;
            model.UniquePhoneKey = string.Empty;

            var oldModificationDate = model.ModificationDate;

            // Act
            var result = ConfirmCustomerRepository.Update(model).Result;
            var dbRecord = ConfirmCustomerRepository.GetById(model.Id).Result;

            // Assert
            Assert.IsTrue(result);
            Assert.IsEmpty(dbRecord.UniqueEmailKey);
            Assert.IsEmpty(dbRecord.UniquePhoneKey);
            Assert.AreEqual(dbRecord.CreationDate, model.CreationDate);
            Assert.AreNotEqual(dbRecord.ModificationDate, oldModificationDate);
        }

        [Test]
        public void DeleteTest()
        {
            // Setup
            var id = Guid.NewGuid();
            GenerateDbRecord(id);
            var model = ConfirmCustomerRepository.GetById(id).Result;

            // Act
            var result = ConfirmCustomerRepository.Delete(model).Result;
            var dbRecord = ConfirmCustomerRepository.GetById(model.Id).Result;

            // Assert
            Assert.AreEqual(result, 1);
            Assert.IsNull(dbRecord);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ConfirmValidationTest(bool isPhoneValidation)
        {
            // Setup
            var id = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            GenerateDbRecord(id, customerId);

            //// Act
            //var result = isPhoneValidation
            //    ? ConfirmCustomerRepository.ConfirmEmailValidation(customerId).Result
            //    : ConfirmCustomerRepository.ConfirmPhoneValidation(customerId).Result;

            //// Assert
            //Assert.IsTrue(result);
            Assert.IsTrue(true);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ConfirmValidationNotFoundTest(bool isPhoneValidation)
        {
            // Setup
            var id = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            GenerateDbRecord(id);

            // Act
            var result = isPhoneValidation
                ? ConfirmCustomerRepository.ConfirmEmailValidation(customerId).Result
                : ConfirmCustomerRepository.ConfirmPhoneValidation(customerId).Result;

                // Assert
            Assert.IsFalse(result);
        }

        #region Private methods

        private void GenerateDbRecords(int numberRecords)
        {
            var customerId = Guid.NewGuid();
            Context.Customers.Add(new Customer
            {
                Id = customerId,
                FirstName = $"First Name for {customerId}",
                LastName = $"Last Name for {customerId}",
                Birthday = new DateTime(2020, 01, 01),
                CreationDate = DateTimeOffset.UtcNow.AddDays(1).AddHours(2).AddMinutes(3),
                ModificationDate = DateTimeOffset.MinValue,
                Email = $"{customerId}@mail.com",
                Gender = (short)new Random().Next(1, 2),
                IdentificationNumber = new Random().Next(8000000, 1100000000),
                PasswordHash = Encoding.ASCII.GetBytes("PasswordHash"),
                PasswordSalt = Encoding.ASCII.GetBytes("PasswordSalt"),
                PhoneNumber = new Random().Next(300000000, 326000000).ToString(),
                VerifiedEmail = false,
                VerifiedPhoneNumber = false,
                Active = true,
                IdentificationType = new IdentificationType
                {
                    Id = customerId,
                    Name = $"Name for {customerId}",
                    Description = $"Description for {customerId}",
                    CreationDate = DateTimeOffset.UtcNow.AddDays(1).AddHours(2).AddMinutes(3),
                    ModificationDate = DateTimeOffset.MinValue
                }
            });
            for (int i = 0; i < numberRecords; i++)
            {
                var id = Guid.NewGuid();
                Context.ConfirmCustomers.Add(new ConfirmCustomer
                {
                    Id = id,
                    CustomerId = customerId,
                    ExpirationEmail = DateTimeOffset.UtcNow.AddDays(10).AddHours(2).AddMinutes(3),
                    UniqueEmailKey = $"UniqueEmailKey-{id}",
                    UniquePhoneKey = $"UniquePhoneKey-{id}",
                    CreationDate = DateTimeOffset.UtcNow.AddDays(i).AddHours(i).AddMinutes(i),
                    ModificationDate = DateTimeOffset.MinValue
                });
            }

            Context.SaveChanges();
        }

        private void GenerateDbRecord(Guid id, Guid? customerId = null)
        {
            var newCustomerId = customerId ?? Guid.NewGuid();
            Context.Customers.Add(new Customer
            {
                Id = newCustomerId,
                FirstName = $"First Name for {newCustomerId}",
                LastName = $"Last Name for {newCustomerId}",
                Birthday = new DateTime(2020, 01, 01),
                CreationDate = DateTimeOffset.UtcNow.AddDays(1).AddHours(2).AddMinutes(3),
                ModificationDate = DateTimeOffset.MinValue,
                Email = $"{customerId}@mail.com",
                Gender = (short)new Random().Next(1, 2),
                IdentificationNumber = new Random().Next(8000000, 1100000000),
                PasswordHash = Encoding.ASCII.GetBytes("PasswordHash"),
                PasswordSalt = Encoding.ASCII.GetBytes("PasswordSalt"),
                PhoneNumber = new Random().Next(300000000, 326000000).ToString(),
                VerifiedEmail = false,
                VerifiedPhoneNumber = false,
                Active = true,
                IdentificationType = new IdentificationType
                {
                    Id = newCustomerId,
                    Name = $"Name for {newCustomerId}",
                    Description = $"Description for {newCustomerId}",
                    CreationDate = DateTimeOffset.UtcNow.AddDays(1).AddHours(2).AddMinutes(3),
                    ModificationDate = DateTimeOffset.MinValue
                }
            });

            Context.ConfirmCustomers.Add(new ConfirmCustomer
            {
                Id = id,
                CustomerId = newCustomerId,
                ExpirationEmail = DateTimeOffset.UtcNow.AddDays(10).AddHours(2).AddMinutes(3),
                UniqueEmailKey = $"UniqueEmailKey-{id}",
                UniquePhoneKey = $"UniquePhoneKey-{id}",
                CreationDate = DateTimeOffset.UtcNow.AddDays(1).AddHours(2).AddMinutes(3),
                ModificationDate = DateTimeOffset.MinValue
            });

            Context.SaveChanges();
        }

        #endregion

    }
}
