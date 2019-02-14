using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BRM.DAO.Repository
{
    public interface IRepository<T>
    {
        //Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null);
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes);

        Task<IQueryable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(long id);
        Task<bool> ExistsByIdAsync(long id);
        Task<T> InsertAsync(T entity);
        Task<ICollection<T>> InsertManyAsync(ICollection<T> entity);
        Task<ICollection<T>> RemoveManyAsync(ICollection<T> entity);
        Task<T> UpdateAsync(T entity);

        Task<T> RemoveAsync(T entity);
        //Task SaveChangesAsync();
    }
}