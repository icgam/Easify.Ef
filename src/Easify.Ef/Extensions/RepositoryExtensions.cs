using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Easify.Extensions.Specifications;
using EfCore.UnitOfWork;

namespace Easify.Ef.Extensions
{
    public static class RepositoryExtensions
    {
        public static IEnumerable<TEntity> GetList<TEntity>(this IRepository<TEntity> repository,
            Specification<TEntity> spec,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null) where TEntity : class
        {
            return repository.GetList(spec.ToExpression(), extend);
        }

        public static async Task<IEnumerable<TEntity>> GetListAsync<TEntity>(this IRepository<TEntity> repository,
            Specification<TEntity> spec,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null) where TEntity : class
        {
            return await repository.GetListAsync(spec.ToExpression(), extend);
        }

        public static TEntity GetFirstOrDefault<TEntity>(this IRepository<TEntity> repository,
            Specification<TEntity> spec,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null) where TEntity : class
        {
            return repository.GetFirstOrDefault(spec.ToExpression(), extend);
        }

        public static async Task<TEntity> GetFirstOrDefaultAsync<TEntity>(this IRepository<TEntity> repository,
            Specification<TEntity> spec,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null) where TEntity : class
        {
            return await repository.GetFirstOrDefaultAsync(spec.ToExpression(), extend);
        }        
    }
}