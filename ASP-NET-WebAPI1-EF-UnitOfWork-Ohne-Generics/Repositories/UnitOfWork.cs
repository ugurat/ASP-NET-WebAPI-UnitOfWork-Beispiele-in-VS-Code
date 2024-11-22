using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PersonApi.Data;

namespace PersonApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PersonDbContext _context;

        private IPersonRepository _personRepository;

        public UnitOfWork(PersonDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Ruft das PersonRepository ab oder erstellt es.
        /// </summary>
        public IPersonRepository PersonRepository
        {
            get
            {
                return _personRepository ??= new PersonRepository(_context);
            }
        }

        /// <summary>
        /// Speichert Änderungen synchron.
        /// </summary>
        public void Commit()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// Speichert Änderungen asynchron.
        /// </summary>
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
