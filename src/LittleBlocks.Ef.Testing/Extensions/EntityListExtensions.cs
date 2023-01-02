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

namespace LittleBlocks.Ef.Testing.Extensions;

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
