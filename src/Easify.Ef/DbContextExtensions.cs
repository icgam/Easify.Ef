using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Easify.Ef
{
    public static class DbContextExtensions
    {
        public static IServiceCollection AddSqlDbContext<TDbContext>(this IServiceCollection services,
            string connectionString) where TDbContext : DbContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddDbContext<TDbContext>(options => options.UseSqlServer(connectionString));
            services.AddUnitOfWork<TDbContext>();

            return services;
        }        
        
        public static IServiceCollection AddSqlDbContext<TDbContext>(this IServiceCollection services,
            string connectionString, Action<SqlServerDbContextOptionsBuilder> sqlOptionsAction) where TDbContext : DbContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddDbContext<TDbContext>(options => options.UseSqlServer(connectionString, sqlOptionsAction)); 
            services.AddUnitOfWork<TDbContext>();

            return services;
        }
    }
}