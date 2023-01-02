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

namespace LittleBlocks.Ef;

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
