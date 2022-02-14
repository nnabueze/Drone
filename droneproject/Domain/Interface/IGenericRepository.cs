using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace droneproject.Domain.Interface
{
    public interface IGenericRepository<T> where T : class, new()
    {
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> FindAllInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task<T> FindSingleInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task<IEnumerable<T>> FindBy(Expression<Func<T, bool>> predicate);

        Task<T> Add(T entity);

        Task<List<T>> Add(List<T> entity);

        public IEnumerable<T> FindAll();

        public T FirstOrDefault();

        void Update(T entity);

        T FindFirst(Expression<Func<T, bool>> predicate);

        void Delete(T entity);

        void DeleteWhere(Expression<Func<T, bool>> predicate);

        Task CommitAsync();
    }
}
