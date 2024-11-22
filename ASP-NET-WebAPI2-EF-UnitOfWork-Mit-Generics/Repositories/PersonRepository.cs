using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PersonApi.Data;

namespace PersonApi.Repositories
{
    
    public class PersonRepository : GenericRepository<Person>,  IPersonRepository
    {

        private readonly PersonDbContext _personContext;

         public PersonRepository(PersonDbContext context) : base(context)
         {
             _personContext = context;
         }

        public async Task<IEnumerable<Person>> SearchAsync(string searchTerm)
         {
             return await _personContext.People
                 .Where(p => p.Vorname.Contains(searchTerm) ||
                             p.Nachname.Contains(searchTerm) ||
                             p.Email.Contains(searchTerm))
                 .ToListAsync();
         }

    }
}
