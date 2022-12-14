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

namespace LittleBlocks.Ef;

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
                auditable.LastModifiedBy = userName;
                ;
                auditable.LastModifiedDate = DateTime.Now;
            }
        }
    }

    private static void SetEntryPropertyValue(EntityEntry entry, string propertyName, object value)
    {
        var property = entry.Properties.FirstOrDefault(p =>
            p.Metadata.Name.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase));
        if (property != null)
            property.CurrentValue = value;
    }
}
