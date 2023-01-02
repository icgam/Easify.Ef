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

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LittleBlocks.Ef.UnitOfWork
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> FromSql(string sql, params object[] parameters);
        TEntity Find(params object[] keyValues);
        ValueTask<TEntity> FindAsync(params object[] keyValues);
        ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken);
        int Count(Expression<Func<TEntity, bool>> predicate = null);
        void Insert(TEntity entity);
        void Insert(params TEntity[] entities);
        void Insert(IEnumerable<TEntity> entities);
        ValueTask<EntityEntry<TEntity>> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task InsertAsync(params TEntity[] entities);
        Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        void Update(TEntity entity);
        void Update(params TEntity[] entities);
        void Update(IEnumerable<TEntity> entities);
        void Delete(object id);
        void Delete(TEntity entity);
        void Delete(params TEntity[] entities);
        void Delete(IEnumerable<TEntity> entities);

        Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extendBy = null);

        Task<IEnumerable<TEntity>> GetListAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> extendBy = null);

        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extendBy = null);

        IEnumerable<TEntity> GetList(Func<IQueryable<TEntity>, IQueryable<TEntity>> extendBy = null);

        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extendBy = null);

        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extendBy = null);

        Task<IEnumerable<TProjectedEntity>> GetProjectedListAsync<TProjectedEntity>(Expression<Func<TEntity, bool>> predicate,
            Func<TEntity, TProjectedEntity> projectedBy,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extendBy = null);

        Task<IEnumerable<TProjectedEntity>> GetProjectedListAsync<TProjectedEntity>(
            Func<TEntity, TProjectedEntity> projectedBy,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extendBy = null);

        IEnumerable<TProjectedEntity> GetProjectedList<TProjectedEntity>(Expression<Func<TEntity, bool>> predicate,
            Func<TEntity, TProjectedEntity> projectedBy,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extendBy = null);

        IEnumerable<TProjectedEntity> GetProjectedList<TProjectedEntity>(Func<TEntity, TProjectedEntity> projectedBy,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> extendBy = null);
    }
}