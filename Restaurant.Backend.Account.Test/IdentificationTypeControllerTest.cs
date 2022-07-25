using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Restaurant.Backend.Account.Controllers;
using Restaurant.Backend.Dto.Entities;
using Restaurant.Backend.Entities.Entities;
using System;
using System.Collections.Generic;


namespace Restaurant.Backend.Account.Test
{
    public class IdentificationTypeControllerTest : BaseControllerTest<IdentificationTypeController>
    {
        [Test]
        public void GetAllTest()
        {
            // Setup
            GenerateDbRecords(10);
            var controller = new IdentificationTypeController(LoggerController.Object, IdentificationTypeDomain, Mapper);

            // Act
            var result = controller.GetAll().Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(typeof(OkObjectResult), result.GetType());
            Assert.AreSame(typeof(List<IdentificationTypeDto>), (result as OkObjectResult)?.Value.GetType());
            Assert.AreEqual(10, ((result as OkObjectResult)?.Value as List<IdentificationTypeDto>)?.Count);
        }

        [Test]
        public void GetTest()
        {
            // Setup
            var id = Guid.NewGuid();
            GenerateDbRecord(id, "Passport", "Passport Description");

            var controller = new IdentificationTypeController(LoggerController.Object, IdentificationTypeDomain, Mapper);

            // Act
            var result = controller.Get(id).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(typeof(OkObjectResult), result.GetType());
            Assert.AreSame(typeof(IdentificationTypeDto), (result as OkObjectResult)?.Value.GetType());
            Assert.AreEqual(id, ((result as OkObjectResult)?.Value as IdentificationTypeDto)?.Id);
        }

        [Test]
        public void CreateTest()
        {
            // Setup
            var modelDto = new IdentificationTypeDto
            {
                Id = Guid.NewGuid(),
                Name = "Passport",
                Description = string.Empty
            };

            var controller = new IdentificationTypeController(LoggerController.Object, IdentificationTypeDomain, Mapper);

            // Act
            var result = controller.Create(modelDto).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(typeof(CreatedResult), result.GetType());
        }

        [Test]
        public void UpdateNotFoundTest()
        {
            // Setup
            var modelDto = new IdentificationTypeDto
            {
                Id = Guid.NewGuid(),
                Name = "Passport",
                Description = string.Empty
            };

            var controller = new IdentificationTypeController(LoggerController.Object, IdentificationTypeDomain, Mapper);

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
            GenerateDbRecord(id, "Passport", "Passport Description");

            var modelDto = new IdentificationTypeDto
            {
                Id = id,
                Name = "Updated Passport",
                Description = string.Empty
            };

            var controller = new IdentificationTypeController(LoggerController.Object, IdentificationTypeDomain, Mapper);

            // Act
            var result = controller.Update(modelDto).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(typeof(OkObjectResult), result.GetType());
            Assert.AreSame(typeof(IdentificationTypeDto), (result as OkObjectResult)?.Value.GetType());
            Assert.AreEqual(modelDto.Name, ((result as OkObjectResult)?.Value as IdentificationTypeDto)?.Name);
            Assert.AreEqual(modelDto.Description, ((result as OkObjectResult)?.Value as IdentificationTypeDto)?.Description);
        }

        [Test]
        public void DeleteNotFoundTest()
        {
            // Setup
            var modelDto = new IdentificationTypeDto
            {
                Id = Guid.NewGuid(),
                Name = "Passport",
                Description = string.Empty
            };

            var controller = new IdentificationTypeController(LoggerController.Object, IdentificationTypeDomain, Mapper);

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
            GenerateDbRecord(id, "Passport", "Passport Description");
            var modelDto = new IdentificationTypeDto
            {
                Id = id,
                Name = "Passport",
                Description = string.Empty
            };

            var controller = new IdentificationTypeController(LoggerController.Object, IdentificationTypeDomain, Mapper);

            // Act
            var result = controller.Delete(modelDto).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(typeof(OkObjectResult), result.GetType());
        }

        #region Private methods

        private void GenerateDbRecords(int numberRecords)
        {
            for (int i = 0; i < numberRecords; i++)
            {
                var id = Guid.NewGuid();
                Context.IdentificationTypes.Add(new IdentificationType
                {
                    Id = id,
                    Name = $"Name for {id}",
                    Description = $"Description for {id}",
                    CreationDate = DateTimeOffset.UtcNow.AddDays(i).AddHours(i).AddMinutes(i),
                    ModificationDate = DateTimeOffset.MinValue
                });
            }

            Context.SaveChanges();
        }

        private void GenerateDbRecord(Guid id, string name, string description)
        {
            Context.IdentificationTypes.Add(new IdentificationType
            {
                Id = id,
                Name = name,
                Description = description,
                CreationDate = DateTimeOffset.UtcNow.AddDays(1).AddHours(2).AddMinutes(3),
                ModificationDate = DateTimeOffset.MinValue
            });

            Context.SaveChanges();
        }

        #endregion
    }
}
