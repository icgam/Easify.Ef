using System;
using System.Collections.Generic;
using EfCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Easify.Ef.Testing.Extensions
{
    public static class EntityListExtensions
    {
        public static TDbContext ToDbContext<T, TDbContext>(this IEnumerable<T> entities,
            Action<TDbContext> action = null)
            where T : class, new()
            where TDbContext : DbContextBase
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            var name = $"{typeof(TDbContext).Name}_{Guid.NewGuid()}";
            
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<TDbContext>()
                .UseInMemoryDatabase(name)
                .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            var dbContext =
                Activator.CreateInstance(typeof(TDbContext), options, new FakeOperationContext()) as TDbContext;
            dbContext?.AddRange(entities);
            dbContext?.SaveChanges();

            action?.Invoke(dbContext);

            return dbContext;
        }

         public static IUnitOfWork ToUnitOfWork<T, TDbContext>(this IEnumerable<T> entities,
            Action<TDbContext> action = null)
            where T : class, new()
            where TDbContext : DbContextBase
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            var dbContext = entities.ToDbContext(action);
            var unitOfWork = new UnitOfWork<DbContext>(dbContext);

            return unitOfWork;
        }
        
        public static IUnitOfWork<TDbContext> ToTypedUnitOfWork<T, TDbContext>(this IEnumerable<T> entities,
            Action<TDbContext> action = null) where T : class, new() where TDbContext : DbContextBase
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            return new UnitOfWork<TDbContext>(entities.ToDbContext(action));
        }
    }
}