using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hermes.Services.EmailSenderService.Domain.Repositories
{
    public interface IRepository<T, TPrimaryKey> where T : class, new()
    {
        IQueryable<T> GetQueryable();
        Task<T> Get(Expression<Func<T, bool>> predicate);
        Task<T> Get(Expression<Func<T, object>>[] includes);
        Task<T> Get(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAll();
        Task<List<T>> GetAll(Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAll(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAll(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[] includes);
        Task<List<T>> SearchBy(Expression<Func<T, bool>> searchBy, params Expression<Func<T, object>>[] includes);
        Task<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<bool> Any(Expression<Func<T, bool>> predicate);
        Task<bool> All(Expression<Func<T, bool>> predicate);
        Task<int> Count(Expression<Func<T, bool>> predicate);
        Task Insert(T entity);
        Task Insert(params T[] entities);
        Task Insert(IEnumerable<T> entities);
        void Update(T entity);
        void Update(params T[] entities);
        void Update(IEnumerable<T> entities);
        void Delete(Expression<Func<T, bool>> identity);
        void Delete(T entity);
        void Delete(params T[] entities);
        void Delete(IEnumerable<T> entities);
    }
}