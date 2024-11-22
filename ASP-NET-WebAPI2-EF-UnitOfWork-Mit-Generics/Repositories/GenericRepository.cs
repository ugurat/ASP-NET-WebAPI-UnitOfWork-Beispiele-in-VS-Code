 using System.Collections.Generic;
 using System.Threading.Tasks;
 using Microsoft.EntityFrameworkCore;

 namespace PersonApi.Repositories
 {
     public class GenericRepository<T> : IGenericRepository<T> where T : class
     {
         private readonly DbContext _context;

         public GenericRepository(DbContext context)
         {
             _context = context;
         }

         public async Task<IEnumerable<T>> GetAllAsync()
         {
             return await _context.Set<T>().ToListAsync();
         }

         public async Task<T> GetByIdAsync(int id)
         {
             return await _context.Set<T>().FindAsync(id);
         }

         public async Task AddAsync(T entity)
         {
             await _context.Set<T>().AddAsync(entity);
         }

         public void Update(T entity)
         {
             _context.Set<T>().Update(entity);
         }

         public void Delete(T entity)
         {
             _context.Set<T>().Remove(entity);
         }
     }
 }