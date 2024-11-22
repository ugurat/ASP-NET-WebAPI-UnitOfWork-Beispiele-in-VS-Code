 using System;
 using System.Threading.Tasks;
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

         public IPersonRepository PersonRepository
         {
             get { return _personRepository ??= new PersonRepository(_context); }
         }

         public void Commit()
         {
             _context.SaveChanges();
         }

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