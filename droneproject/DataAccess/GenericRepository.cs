using droneproject.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace droneproject.DataAccess
{
    public class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : class, new()
    {
        private ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T> Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);

            return entity;
        }

        public virtual IEnumerable<T> FindAll()
        {
            return _context.Set<T>().AsEnumerable();
        }

        public virtual async Task<List<T>> Add(List<T> entity)
        {
            foreach (var item in entity)
            {
                await _context.Set<T>().AddAsync(item);
            }

            return entity;
        }

        public virtual async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);

            dbEntityEntry.State = EntityState.Deleted;
        }

        public void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);

            foreach (var entity in entities)
            {
                _context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }


        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public async Task<IEnumerable<T>> FindAllInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await GetAllInclude(predicate, includeProperties).ToListAsync();
        }

        public virtual IQueryable<T> GetAllInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return (IQueryable<T>)query.Where(predicate);
        }

        public virtual async Task<IEnumerable<T>> FindBy(Expression<Func<T, bool>> predicate)
        {
            return await FindAllWhere(predicate).ToListAsync();
        }

        public virtual IQueryable<T> FindAllWhere(Expression<Func<T, bool>> predicate)
        {
            return (IQueryable<T>)_context.Set<T>().Where(predicate);
        }

        public T FindFirst(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public async Task<T> FindSingleInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.Where(predicate).FirstOrDefaultAsync();
        }

        public T FirstOrDefault()
        {
            return _context.Set<T>().FirstOrDefault();
        }

        public void Update(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);

            dbEntityEntry.State = EntityState.Modified;
        }

        public void Dispose()
        {
            _context.Dispose();

            _context = null;
        }
    }
}
