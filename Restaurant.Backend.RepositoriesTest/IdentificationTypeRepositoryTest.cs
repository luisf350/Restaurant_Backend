using NUnit.Framework;
using Restaurant.Backend.Entities.Entities;
using System;
using System.Linq;

namespace Restaurant.Backend.RepositoriesTest
{
    public class IdentificationTypeRepositoryTest : BaseRepositoryTest
    {
        [Test]
        public void GetAllTest()
        {
            // Setup
            GenerateDbRecords(10);

            // Act
            var result = IdentificationTypeRepository.GetAll().Result;

            // Assert
            Assert.AreEqual(result.Count(), 10);
        }

        [Test]
        public void GetTest()
        {
            // Setup
            var id = Guid.NewGuid();
            GenerateDbRecord(id, $"Name for {id}", $"Description for {id}");

            // Act
            var result = IdentificationTypeRepository.GetById(id).Result;

            // Assert
            Assert.AreEqual(result.Id, id);
        }

        [Test]
        public void GetNotFoundTest()
        {
            // Setup
            var id = Guid.NewGuid();

            // Act
            var result = IdentificationTypeRepository.GetById(id).Result;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void CreateTest()
        {
            // Setup
            var id = Guid.NewGuid();
            var model = new IdentificationType
            {
                Id = id,
                Name = $"Name for {id}",
                Description = $"Description for {id}",
                CreationDate = DateTimeOffset.UtcNow.AddDays(1).AddHours(2).AddMinutes(3)
            };

            // Act
            var result = IdentificationTypeRepository.Create(model).Result;
            var dbRecord = IdentificationTypeRepository.GetById(model.Id).Result;

            // Assert
            Assert.AreEqual(result, 1);
            Assert.AreEqual(dbRecord.Name, model.Name);
            Assert.AreEqual(dbRecord.Description, model.Description);
            Assert.AreEqual(dbRecord.CreationDate, model.CreationDate);
            Assert.AreEqual(dbRecord.ModificationDate, model.ModificationDate);
        }

        [Test]
        public void UpdateTest()
        {
            // Setup
            var id = Guid.NewGuid();
            const string newName = "Updated Name";
            GenerateDbRecord(id, $"Name for {id}", $"Description for {id}");

            var model = IdentificationTypeRepository.GetById(id).Result;
            model.Name = newName;
            model.Description = string.Empty;

            var oldModificationDate = model.ModificationDate;

            // Act
            var result = IdentificationTypeRepository.Update(model).Result;
            var dbRecord = IdentificationTypeRepository.GetById(model.Id).Result;

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(dbRecord.Name, newName);
            Assert.AreEqual(dbRecord.Description, string.Empty);
            Assert.AreEqual(dbRecord.CreationDate, model.CreationDate);
            Assert.AreNotEqual(dbRecord.ModificationDate, oldModificationDate);
        }

        [Test]
        public void DeleteTest()
        {
            // Setup
            var id = Guid.NewGuid();
            GenerateDbRecord(id, $"Name for {id}", $"Description for {id}");
            var model = IdentificationTypeRepository.GetById(id).Result;

            // Act
            var result = IdentificationTypeRepository.Delete(model).Result;
            var dbRecord = IdentificationTypeRepository.GetById(model.Id).Result;

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
