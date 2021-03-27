using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hermes.Services.EmailSenderService.Domain.Repositories
{
    public class
        EfCoreBaseRepository<T, TPrimaryKey, TContext> : IRepository<T,
            TPrimaryKey>
        where T : class, new()
        where TContext : DbContext, new()
    {
        private readonly IServiceProvider _serviceProvider;
        private IConfiguration _configuration;

        public EfCoreBaseRepository(IConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public IQueryable<T> GetQueryable()
        {
            var dbContext = _serviceProvider.GetRequiredService<TContext>();
            return dbContext.Set<T>().AsQueryable();
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var dbSet = dbContext.Set<T>();
            var entity = await dbSet.FirstOrDefaultAsync(predicate);
            return entity;
        }

        public async Task<T> Get(Expression<Func<T, object>>[] includes)
        {
            var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var dbSet = dbContext.Set<T>();
            var queryable = dbSet.AsQueryable();
            foreach (var includeExpression in includes) queryable = queryable.Include(includeExpression);

            var entity = await queryable.FirstOrDefaultAsync();
            return entity;
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>>[] includes)
        {
            var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var dbSet = dbContext.Set<T>();
            var queryable = dbSet.AsQueryable();
            foreach (var includeExpression in includes) queryable = queryable.Include(includeExpression);

            var entity = await queryable.FirstOrDefaultAsync(predicate);
            return entity;
        }

        public async Task<List<T>> GetAll()
        {
            var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var dbSet = dbContext.Set<T>();
            var entities = dbSet.ToList();
            return entities;
        }

        public async Task<List<T>> GetAll(Expression<Func<T, object>>[] includes)
        {
            var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var dbSet = dbContext.Set<T>();
            var queryable = dbSet.AsQueryable();
            foreach (var includeExpression in includes) queryable = queryable.Include(includeExpression);

            var entities = queryable.ToList();
            return entities;
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> predicate)
        {
            var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var dbSet = dbContext.Set<T>();
            var entities = dbSet.Where(predicate).ToList();
            return entities;
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>>[] includes)
        {
            var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var dbSet = dbContext.Set<T>();
            var queryable = dbSet.AsQueryable();
            foreach (var includeExpression in includes) queryable = queryable.Include(includeExpression);

            var entities = queryable.Where(predicate).ToList();
            return entities;
        }

        public async Task<List<T>> SearchBy(Expression<Func<T, bool>> searchBy,
            params Expression<Func<T, object>>[] includes)
        {
            var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var dbSet = dbContext.Set<T>();
            var queryable = dbSet.AsQueryable();
            foreach (var includeExpression in includes) queryable = queryable.Include(includeExpression);

            var entities = queryable.Where(searchBy).ToList();
            return entities;
        }

        public async Task<T> FindBy(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes)
        {
            var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var dbSet = dbContext.Set<T>();
            var queryable = dbSet.AsQueryable();
            foreach (var includeExpression in includes) queryable = queryable.Include(includeExpression);

            var entities = queryable.Where(predicate).FirstOrDefault();
            return entities;
        }

        public async Task<bool> Any(Expression<Func<T, bool>> predicate)
        {
            var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var dbSet = dbContext.Set<T>();
            var queryable = dbSet.AsQueryable();
            return queryable.Any(predicate);
        }

        public async Task<bool> All(Expression<Func<T, bool>> predicate)
        {
            var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var dbSet = dbContext.Set<T>();
            var queryable = dbSet.AsQueryable();
            return queryable.All(predicate);
        }

        public async Task<int> Count(Expression<Func<T, bool>> predicate)
        {
            var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var dbSet = dbContext.Set<T>();
            var queryable = dbSet.AsQueryable();
            return queryable.Count(predicate);
        }

        public async Task Insert(T entity)
        {
            var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var dbSet = dbContext.Set<T>();
            await dbSet.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task Insert(params T[] entities)
        {
            var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var dbSet = dbContext.Set<T>();
            await dbSet.AddRangeAsync(entities);
            await dbContext.SaveChangesAsync();
        }

        public async Task Insert(IEnumerable<T> entities)
        {
            var auditedEntities = entities.ToArray();
            await Insert(auditedEntities);
        }

        public void Update(T entity)
        {
            using var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var entry = dbContext.Entry(entity);
            entry.State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public void Update(params T[] entities)
        {
            using var dbContext = _serviceProvider.GetRequiredService<TContext>();

            foreach (var entity in entities)
            {
                var entry = dbContext.Entry(entity);
                entry.State = EntityState.Modified;
            }

            dbContext.SaveChanges();
        }

        public void Update(IEnumerable<T> entities)
        {
            using var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var fullAuditedEntities = entities as T[] ?? entities.ToArray();
            foreach (var entity in fullAuditedEntities)
            {
                var entry = dbContext.Entry(entity);
                entry.State = EntityState.Modified;
            }

            dbContext.SaveChanges();
        }


        public void Delete(Expression<Func<T, bool>> identity)
        {
            using var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var dbSet = dbContext.Set<T>();
            var queryable = dbSet.AsQueryable();
            var entities = queryable.Where(identity).ToList();
            dbSet.RemoveRange(entities);
            dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
                return;
            using var dbContext = _serviceProvider.GetRequiredService<TContext>();
            dbContext.Set<T>().Remove(entity);
            dbContext.SaveChanges();
        }

        public void Delete(params T[] entities)
        {
            using var dbContext = _serviceProvider.GetRequiredService<TContext>();
            var dbSet = dbContext.Set<T>();
            dbSet.RemoveRange(entities);
            dbContext.SaveChanges();
        }

        public void Delete(IEnumerable<T> entities)
        {
            Delete(entities.ToArray());
        }
    }
}