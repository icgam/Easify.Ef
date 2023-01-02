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

namespace LittleBlocks.Ef.Extensions;

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
