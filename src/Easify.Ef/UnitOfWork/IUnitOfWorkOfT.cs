﻿using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Easify.Ef.UnitOfWork
{
    public interface IUnitOfWork<out TContext> : IUnitOfWork where TContext : DbContext {
        TContext DbContext { get; }
        Task<int> SaveChangesAsync(bool ensureAutoHistory = false, params IUnitOfWork[] unitOfWorks);
    }
}
