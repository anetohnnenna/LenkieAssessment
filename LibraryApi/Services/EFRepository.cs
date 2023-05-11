using LibraryApi.Data;
using LibraryApi.Entities;
using LibraryApi.Interface;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryApi.Services
{
    public class EFRepository<T> : IRepository<T> where T : BaseEntity
    {
        public readonly ApplicationDbContext _dbContext;

        public EFRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

        }
    
      
        public virtual async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbContext.Set<T>().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<T> FetchRecord(Expression<Func<T, bool>> predicate)
        {
           
          return await _dbContext.Set<T>().Where(predicate).FirstOrDefaultAsync();
     
        }     


        public async Task Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

    }
}
