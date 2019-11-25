using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Easify.Ef.ComponentModel;
using Easify.Ef.Extensions;
using ICG.Core.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Easify.Ef
{
    public abstract class DbContextBase : DbContext
    {
        private readonly IOperationContext _operationContext;

        [Obsolete("It will be removed in the next version. Use the constructor with IOperationContext instead.")]
        protected DbContextBase(DbContextOptions options, IRequestContext requestContext) : base(options)
        {
            _operationContext = requestContext ?? throw new ArgumentNullException(nameof(requestContext));
        }

        protected DbContextBase(DbContextOptions options, IOperationContext operationContext) : base(options)
        {
            _operationContext = operationContext ?? throw new ArgumentNullException(nameof(operationContext));
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAuditInfo();

            ChangeTracker.AutoDetectChangesEnabled = false;

            var result = base.SaveChanges(acceptAllChangesOnSuccess);

            ChangeTracker.AutoDetectChangesEnabled = true;

            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            ApplyAuditInfo();

            ChangeTracker.AutoDetectChangesEnabled = false;

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            ChangeTracker.AutoDetectChangesEnabled = true;

            return result;
        }

        private void ApplyAuditInfo()
        {
            ChangeTracker.DetectChanges();

            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            var userName = _operationContext.User?.GetUserName() ?? "Anonymous";

            foreach (var entry in entries)
            {
                if (!(entry.Entity is IAuditable auditable))
                {
                    SetEntryPropertyValue(entry, nameof(Auditable.LastModifiedBy), userName);
                    SetEntryPropertyValue(entry, nameof(Auditable.LastModifiedDate), DateTime.Now);
                }
                else
                {
                    auditable.LastModifiedBy = userName; ;
                    auditable.LastModifiedDate = DateTime.Now;
                }
            }
        }

        private void SetEntryPropertyValue(EntityEntry entry, string propertyName, object value)
        {
            var property = entry.Properties.FirstOrDefault(p => p.Metadata.Name.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase));
            if (property != null)
                property.CurrentValue = value;
        }
    }
}