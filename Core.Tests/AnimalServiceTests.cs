using System.Collections.Generic;
using System.Linq;
using Contracts.Enums;
using Contracts.Models;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Core.Tests
{
    [TestClass]
    public class AnimalServiceTests
    {
        private Database _database;
        private Mock<ISession> _session;

        [TestInitialize]
        public void Init()
        {
            _session = new Mock<ISession>();
            _database = new Database(_session.Object);
        }

        [TestMethod]
        public void get_all_animals()
        {
            var entities = new List<Animal>
            {
                new Animal {Id = 1, Name = "Gertrude", CommonName = CommonName.Goat},
                new Animal {Id = 2, Name = "Leonard", CommonName = CommonName.Llama},
                new Animal {Id = 2, Name = "Duncan", CommonName = CommonName.MiniatureDonkey}
            };

            _session.Setup(m => m.Query<Animal>(It.IsAny<string>(), null)).Returns(entities);

            var service = new AnimalService(_database);
            var animals = service.GetAllAnimals();

            Assert.AreEqual(entities.Count, animals.Count());
        }

        [TestMethod]
        public void save_animal()
        {
            var animal = new Animal
            {
                Name = "Gary",
                CommonName = CommonName.Goat
            };

            _session.Setup(m => m.Execute(It.IsAny<string>(), It.IsAny<object>())).Verifiable();

            var service = new AnimalService(_database);
            service.Save(animal);

            _session.Verify(m => m.Execute(It.IsAny<string>(), It.IsAny<object>()));
        }
    }
}