using System;
using Easify.Ef.Testing.Extensions;
using ICG.Core.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Easify.Ef.Testing
{
    public static class DbContextExtensions
    {
        public static IServiceCollection AddInMemoryDbContext<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var name = $"{typeof(TDbContext).Name}_{Guid.NewGuid()}";
            return services.AddInMemoryDbContext<TDbContext>(name);
        }        
        
        public static IServiceCollection AddInMemoryDbContext<TDbContext>(this IServiceCollection services, string contextName)
            where TDbContext : DbContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (contextName == null) throw new ArgumentNullException(nameof(contextName));

            var name = contextName;

            services.TryAddTransient<IOperationContext, FakeOperationContext>();
            services.AddDbContext<TDbContext>(options =>
                options.UseInMemoryDatabase(name).ConfigureWarnings(m => m.Ignore(InMemoryEventId.TransactionIgnoredWarning)).EnableSensitiveDataLogging());

            services.AddUnitOfWork<TDbContext>();

            return services;
        }
    }
}