using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ICG.Core.Extensions.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Easify.Ef.Extensions
{
    public static class RepositoryExtensions
    {
        public static IEnumerable<TEntity> GetList<TEntity>(this IRepository<TEntity> repository,
            Specification<TEntity> spec,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null) where TEntity : class
        {
            return repository.GetListQueryInternal(spec, extend).ToList();
        }

        public static async Task<IEnumerable<TEntity>> GetListAsync<TEntity>(this IRepository<TEntity> repository,
            Specification<TEntity> spec,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null) where TEntity : class
        {
            return await repository.GetListQueryInternal(spec, extend).ToListAsync();
        }

        public static IEnumerable<TEntity> GetList<TEntity>(this IRepository<TEntity> repository,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null) where TEntity : class
        {
            return repository.GetListQueryInternal(extend).ToList();
        }

        public static async Task<IEnumerable<TEntity>> GetListAsync<TEntity>(this IRepository<TEntity> repository,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null) where TEntity : class
        {
            return await repository.GetListQueryInternal(extend).ToListAsync();
        }        
        
        public static IEnumerable<TEntity> GetList<TEntity>(this IRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null) where TEntity : class
        {
            return repository.GetListQueryInternal(predicate, extend).ToList();
        }

        public static async Task<IEnumerable<TEntity>> GetListAsync<TEntity>(this IRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null) where TEntity : class
        {
            return await repository.GetListQueryInternal(predicate, extend).ToListAsync();
        }

        public static TEntity GetFirstItemOrDefault<TEntity>(this IRepository<TEntity> repository,
            Specification<TEntity> spec,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null) where TEntity : class
        {
            return repository.GetListQueryInternal(spec, extend).FirstOrDefault();
        }

        public static async Task<TEntity> GetFirstItemOrDefaultAsync<TEntity>(this IRepository<TEntity> repository,
            Specification<TEntity> spec,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null) where TEntity : class
        {
            return await repository.GetListQueryInternal(spec, extend).FirstOrDefaultAsync();
        }        
        
        public static TEntity GetFirstItemOrDefault<TEntity>(this IRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null) where TEntity : class
        {
            return repository.GetListQueryInternal(predicate, extend).FirstOrDefault();
        }

        public static async Task<TEntity> GetFirstItemOrDefaultAsync<TEntity>(this IRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null) where TEntity : class
        {
            return await repository.GetListQueryInternal(predicate, extend).FirstOrDefaultAsync();
        }        
        
        private static IQueryable<TEntity> GetListQueryInternal<TEntity>(this IRepository<TEntity> repository,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null) where TEntity : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));

            return repository.GetListQueryInternal(Specification<TEntity>.All, extend);
        }

        private static IQueryable<TEntity> GetListQueryInternal<TEntity>(this IRepository<TEntity> repository,
            Specification<TEntity> spec,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null) where TEntity : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (spec == null) throw new ArgumentNullException(nameof(spec));

            return repository.GetListQueryInternal(spec.ToExpression(), extend);
        }        
        
        private static IQueryable<TEntity> GetListQueryInternal<TEntity>(this IRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null) where TEntity : class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var queryable = repository.GetAll().Where(predicate);
            if (extend != null)
                queryable = extend(queryable);

            return queryable;
        }        
    }
}