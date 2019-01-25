using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BRM.DAO.Data;
using BRM.DAO.Entities;
using BRM.DAO.Repository;
using Microsoft.EntityFrameworkCore;

namespace BackSide2.DAO.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DataContext _context;

        private readonly DbSet<T> _entities;

        public Repository(DataContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        //protected IQueryable<T> Query
        //{
        //    get { return _context.GetTable<T>(); }
        //}

        //public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null)
        //{
        //    if (predicate != null)
        //        return _entities.AsQueryable().Where(predicate);

        //    return _entities.AsQueryable();
        //}

        //public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null) =>
        //    await Task.Run(() => predicate != null ? _entities.Where(predicate) : _entities);

        public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            return await Task.Run(() =>
            {
                var result = this._entities.AsQueryable();
                if (includes != null) result = includes.Aggregate(result, (current, expression) => current.Include(expression));
                return predicate != null ? result.Where(predicate) : result;
            });
        }


        public async Task<IQueryable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            return await Task.Run(() =>
            {
                var result = this._entities.AsQueryable();
                if (includes != null) result = includes.Aggregate(result, (current, expression) => current.Include(expression));
                return result;
            });
        }

        public async Task<T> GetByIdAsync(long id) =>
            await _entities.FirstOrDefaultAsync(e => e.Id == id);

        public async Task<bool> ExistsByIdAsync(long id) =>
            await _entities.Where(e => e.Id == id).AnyAsync();

        public async Task<T> InsertAsync(T entity)
        {
            if (entity == null)
                throw new NullReferenceException();

            _entities.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)
                throw new NullReferenceException();

            _entities.Update(entity);

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> RemoveAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _entities.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        //public async Task SaveChangesAsync()
        //{
        //    _context.SaveChanges();
        //}
    }
}