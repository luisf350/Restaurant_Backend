using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Restaurant.Backend.Account.Controllers;
using Restaurant.Backend.Entities.Entities;
using System;

namespace Restaurant.Backend.Account.Test
{
    public class CountryControllerTest : BaseControllerTest<CountryController>
    {
        [Test]
        public void EmptyDatabaseGetTest()
        {
            // Setup
            var controller = new CountryController(LoggerController.Object, Mapper, CountryDomain);

            //// Act
            var result = controller.Get().Result;

            //// Assert
            Assert.IsNotNull(result);
            Assert.AreSame(typeof(OkObjectResult), result.GetType());
        }

        [Test]
        public void NotEmptyDatabaseGetTest()
        {
            // Setup
            GenerateDbRecords(10);
            var controller = new CountryController(LoggerController.Object, Mapper, CountryDomain);

            //// Act
            var result = controller.Get().Result;

            //// Assert
            Assert.IsNotNull(result);
            Assert.AreSame(typeof(OkObjectResult), result.GetType());
        }

        #region Private Methods

        private void GenerateDbRecords(int numberRecords)
        {
            for (int i = 0; i < numberRecords; i++)
            {
                var id = Guid.NewGuid();
                Context.Countries.Add(new Country
                {
                    Id = id,
                    Name = $"Name for {id}",
                    CallingCode = i,
                    Capital = $"Capital for {id}",
                    Region = $"Region for {id}",
                    SubRegion = $"SubRegion for {id}",
                    Flag = $"Flag for {id}",
                    CreationDate = DateTimeOffset.UtcNow.AddDays(i).AddHours(i).AddMinutes(i),
                    ModificationDate = DateTimeOffset.MinValue
                });
            }

            Context.SaveChanges();
        }


        #endregion
    }
}