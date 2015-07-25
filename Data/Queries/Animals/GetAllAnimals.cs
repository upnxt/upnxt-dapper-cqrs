using System.Collections.Generic;
using System.Linq;
using Contracts.Models;

namespace Data.Queries.Animals
{
    public class GetAllAnimals : IQuery<IList<Animal>>
    {
        public IList<Animal> Execute(ISession session)
        {
            return session.Query<Animal>("SELECT * FROM Animals").ToList();
        }
    }
}
