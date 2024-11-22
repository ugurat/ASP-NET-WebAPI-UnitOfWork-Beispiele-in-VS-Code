using System.Collections.Generic;
using System.Threading.Tasks;
using PersonApi.Data;

namespace PersonApi.Services
{

    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetPeopleAsync();
        Task<IEnumerable<Person>> SearchPeopleAsync(string searchTerm); // Neue Methode für die Suche


        Task<Person> GetPersonByIdAsync(int id);
        Task AddPersonAsync(Person person);
        Task UpdatePersonAsync(Person person);
        Task DeletePersonAsync(int id);
        
    }

}
