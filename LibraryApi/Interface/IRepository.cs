using LibraryApi.Entities;
using LibraryApi.Models;
using System.Linq.Expressions;

namespace LibraryApi.Interface
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task Add(T entity);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate);

        Task<T> FetchRecord(Expression<Func<T, bool>> predicate);
        Task Update(T entity);
    }
}
