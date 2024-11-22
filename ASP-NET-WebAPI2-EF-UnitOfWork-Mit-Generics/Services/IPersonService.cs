using System.Collections.Generic;
using System.Threading.Tasks;
using PersonApi.Data;

namespace PersonApi.Services
{

    public interface IPersonService : IService<Person>
    {
         Task<IEnumerable<Person>> SearchPeopleAsync(string searchTerm);
         
        // Burada Person'a özgü başka metotlar da tanımlayabilirsiniz.
        
    }

}
