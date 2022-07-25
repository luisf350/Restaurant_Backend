using NUnit.Framework;
using Restaurant.Backend.Entities.Entities;
using System;
using System.Linq;

namespace Restaurant.Backend.RepositoriesTest
{
    public class CountryRepositoryTest : BaseRepositoryTest
    {
        [Test]
        public void GetAllTest()
        {
            // Setup
            GenerateDbRecords(10);

            // Act
            var result = CountryRepository.GetAll().Result;

            // Assert
            Assert.AreEqual(result.Count(), 10);
        }

        [Test]
        public void GetTest()
        {
            // Setup
            var id = Guid.NewGuid();
            GenerateDbRecord(id);

            // Act
            var result = CountryRepository.GetById(id).Result;

            // Assert
            Assert.AreEqual(result.Id, id);
        }

        [Test]
        public void GetNotFoundTest()
        {
            // Setup
            var id = Guid.NewGuid();

            // Act
            var result = CountryRepository.GetById(id).Result;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void CreateTest()
        {
            // Setup
            var id = Guid.NewGuid();
            var model = new Country
            {
                Id = id,
                Name = $"Name for {id}",
                CallingCode = 77,
                Capital = $"Capital for {id}",
                Region = $"Region for {id}",
                SubRegion = $"SubRegion for {id}",
                Flag = $"Url Flag for {id}",
                CreationDate = DateTimeOffset.UtcNow.AddDays(1).AddHours(2).AddMinutes(3),
                ModificationDate = DateTimeOffset.MinValue
            };

            // Act
            var result = CountryRepository.Create(model).Result;
            var dbRecord = CountryRepository.GetById(model.Id).Result;

            // Assert
            Assert.AreEqual(result, 1);
            Assert.AreEqual(dbRecord.Name, model.Name);
            Assert.AreEqual(dbRecord.CallingCode, model.CallingCode);
            Assert.AreEqual(dbRecord.Capital, model.Capital);
            Assert.AreEqual(dbRecord.Region, model.Region);
            Assert.AreEqual(dbRecord.SubRegion, model.SubRegion);
            Assert.AreEqual(dbRecord.Flag, model.Flag);
            Assert.AreEqual(dbRecord.CreationDate, model.CreationDate);
            Assert.AreEqual(dbRecord.ModificationDate, model.ModificationDate);
        }

        [Test]
        public void UpdateTest()
        {
            // Setup
            var id = Guid.NewGuid();
            const string newName = "Updated Name";
            GenerateDbRecord(id);

            var model = CountryRepository.GetById(id).Result;
            model.Name = newName;
            model.Capital = newName;

            var oldModificationDate = model.ModificationDate;

            // Act
            var result = CountryRepository.Update(model).Result;
            var dbRecord = CountryRepository.GetById(model.Id).Result;

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(dbRecord.Name, newName);
            Assert.AreEqual(dbRecord.Capital, newName);
            Assert.AreEqual(dbRecord.CallingCode, model.CallingCode);
            Assert.AreEqual(dbRecord.CreationDate, model.CreationDate);
            Assert.AreNotEqual(dbRecord.ModificationDate, oldModificationDate);
        }

        [Test]
        public void DeleteTest()
        {
            // Setup
            var id = Guid.NewGuid();
            GenerateDbRecord(id);
            var model = CountryRepository.GetById(id).Result;

            // Act
            var result = CountryRepository.Delete(model).Result;
            var dbRecord = CountryRepository.GetById(model.Id).Result;

            // Assert
            Assert.AreEqual(result, 1);
            Assert.IsNull(dbRecord);
        }

        #region Private methods

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
                    Flag = $"Url Flag for {id}",
                    CreationDate = DateTimeOffset.UtcNow.AddDays(i).AddHours(i).AddMinutes(i),
                    ModificationDate = DateTimeOffset.MinValue
                });
            }

            Context.SaveChanges();
        }

        private void GenerateDbRecord(Guid id)
        {
            Context.Countries.Add(new Country
            {
                Id = id,
                Name = $"Name for {id}",
                CallingCode = 77,
                Capital = $"Capital for {id}",
                Region = $"Region for {id}",
                SubRegion = $"SubRegion for {id}",
                Flag = $"Url Flag for {id}",
                CreationDate = DateTimeOffset.UtcNow.AddDays(1).AddHours(2).AddMinutes(3),
                ModificationDate = DateTimeOffset.MinValue
            });

            Context.SaveChanges();
        }

        #endregion

    }
}
