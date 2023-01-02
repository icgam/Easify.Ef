// This software is part of the LittleBlocks.Ef Library
// Copyright (C) 2022 Little Blocks
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using LittleBlocks.Ef.UnitOfWork;

namespace LittleBlocks.Ef.Testing;

public static class DbContextExtensions
{
    public static IServiceCollection AddInMemoryUnitOfWork<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        var name = $"{typeof(TDbContext).Name}_{Guid.NewGuid()}";
        return services.AddInMemoryUnitOfWork<TDbContext>(name);
    }

    private static IServiceCollection AddInMemoryUnitOfWork<TDbContext>(this IServiceCollection services, string contextName)
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
