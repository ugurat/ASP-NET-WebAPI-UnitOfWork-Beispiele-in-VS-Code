using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PersonApi.Data;

namespace PersonApi.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonDbContext _context;

        public PersonRepository(PersonDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _context.People.ToListAsync();
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            return await _context.People.FindAsync(id);
        }

        public async Task AddAsync(Person person)
        {
            await _context.People.AddAsync(person);
        }

        public void Update(Person person)
        {
            _context.People.Update(person);
        }

        public void Delete(Person person)
        {
            _context.People.Remove(person);
        }

        public async Task<IEnumerable<Person>> SearchAsync(string searchTerm)
        {
            return await _context.People
                .Where(p => p.Vorname.Contains(searchTerm) ||
                            p.Nachname.Contains(searchTerm) ||
                            p.Email.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
