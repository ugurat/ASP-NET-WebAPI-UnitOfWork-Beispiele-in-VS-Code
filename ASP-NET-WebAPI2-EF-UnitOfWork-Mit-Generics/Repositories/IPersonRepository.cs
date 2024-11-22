using PersonApi.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonApi.Repositories
{
    public interface IPersonRepository : IGenericRepository<Person>
    {

        // Person'a özel metodlar burada tanımlanabilir
        Task<IEnumerable<Person>> SearchAsync(string searchTerm);
    }
}
