using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonApi.Data;
using PersonApi.Repositories;

namespace PersonApi.Services
{

    public class PersonService : Service<Person>, IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IUnitOfWork unitOfWork, IPersonRepository personRepository) 
            : base(unitOfWork)
        {
            _personRepository = personRepository;
        }

        public async Task<IEnumerable<Person>> SearchPeopleAsync(string searchTerm)
        {
            return await _personRepository.SearchAsync(searchTerm);
        }
    }

}
