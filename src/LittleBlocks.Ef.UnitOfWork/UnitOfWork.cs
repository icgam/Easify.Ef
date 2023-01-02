// Copyright (c) Arch team. All rights reserved.
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

using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LittleBlocks.Ef.UnitOfWork
{
    public sealed class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext> where TContext : DbContext
    {
        private bool _disposed;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(TContext context) => DbContext = context ?? throw new ArgumentNullException(nameof(context));

        public TContext DbContext { get; }

        public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class
        {
            _repositories ??= new Dictionary<Type, object>();

            // TODO: Find a better way of resolving the repository
            if (hasCustomRepository)
            {
                var repository = DbContext.GetService<IRepository<TEntity>>();
                if (repository != null)
                    return repository;
            }

            // TODO: Needs to be thread-safe
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type)) 
                _repositories[type] = new Repository<TEntity>(DbContext);

            return (IRepository<TEntity>)_repositories[type];
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters) => DbContext.Database.ExecuteSqlRaw(sql, parameters);

        public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class => DbContext.Set<TEntity>().FromSqlRaw(sql, parameters);

        public int SaveChanges(bool ensureAutoHistory = false)
        {
            if (ensureAutoHistory) 
                DbContext.EnsureAutoHistory();

            return DbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false)
        {
            if (ensureAutoHistory) 
                DbContext.EnsureAutoHistory();

            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false, params IUnitOfWork[] unitOfWorks)
        {
            using (var ts = new TransactionScope())
            {
                var tasks = unitOfWorks.Select(async unitOfWork => await unitOfWork.SaveChangesAsync(ensureAutoHistory)).ToList();
                var results = await Task.WhenAll(tasks);
                
                var count = results.Sum();
                count += await SaveChangesAsync(ensureAutoHistory);

                ts.Complete();

                return count;
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repositories?.Clear();
                    DbContext.Dispose();
                }
            }

            _disposed = true;
        }

        public void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> callback) => DbContext.ChangeTracker.TrackGraph(rootEntity, callback);
    }
}
