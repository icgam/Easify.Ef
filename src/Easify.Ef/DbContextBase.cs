using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Easify.Ef.ComponentModel;
using Easify.Ef.Extensions;
using Easify.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Easify.Ef
{
    public abstract class DbContextBase : DbContext
    {
        private readonly IOperationContext _operationContext;

        protected DbContextBase(DbContextOptions options, IOperationContext operationContext) : base(options)
        {
            _operationContext = operationContext ?? throw new ArgumentNullException(nameof(operationContext));
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAuditInfo();

            ChangeTracker.AutoDetectChangesEnabled = false;

            var result = base.SaveChanges(acceptAllChangesOnSuccess: acceptAllChangesOnSuccess);

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

            var userName = _operationContext.User?.GetUserName();

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

        private static void SetEntryPropertyValue(EntityEntry entry, string propertyName, object value)
        {
            var property = entry.Properties.FirstOrDefault(p => p.Metadata.Name.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase));
            if (property != null)
                property.CurrentValue = value;
        }
    }
}