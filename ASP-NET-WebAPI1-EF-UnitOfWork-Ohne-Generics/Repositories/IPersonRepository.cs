using PersonApi.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonApi.Repositories
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllAsync();

        Task<Person> GetByIdAsync(int id);

        Task AddAsync(Person person);

        void Update(Person person);

        void Delete(Person person);

        Task<IEnumerable<Person>> SearchAsync(string searchTerm);
    }
}
