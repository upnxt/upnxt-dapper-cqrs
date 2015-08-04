using System.Collections.Generic;
using Contracts.Enums;
using Contracts.Models;
using Data;
using Data.Commands.Animals;
using Data.Queries.Animals;

namespace Core
{
    public interface IAnimalService
    {
        IEnumerable<Animal> GetAllAnimals();
        IEnumerable<Animal> GetAnimalsByCommonName(CommonName commonName);
        void Save(Animal animal);
    }

    public class AnimalService : IAnimalService
    {
        private readonly IDatabase _database;

        public AnimalService(IDatabase database)
        {
            _database = database;
        }

        public IEnumerable<Animal> GetAllAnimals()
        {
            return _database.Query(new GetAllAnimals());
        }

        public IEnumerable<Animal> GetAnimalsByCommonName(CommonName commonName)
        {
            return _database.Query(new GetAnimalsByCommonName(commonName));
        }

        public void Save(Animal animal)
        {
            _database.Execute(new SaveAnimal(animal));
        }
    }
}