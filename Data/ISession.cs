using System.Collections.Generic;

namespace Data
{
    public interface ISession
    {
        IEnumerable<T> Query<T>(string query, object param = null);
        void Execute(string query, object param = null);
    }
}