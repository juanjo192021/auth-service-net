using Authentication.RefreshToken.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Authentication.RefreshToken.Persistence.Interceptors
{
    public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken)
        {
            var context = eventData.Context;
            if (context is not null)
                UpdateEntities(context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        public void UpdateEntities(DbContext context)
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        break;

                    case EntityState.Modified:

                        var softDeleteProperty = entry.Properties
                            .FirstOrDefault(p => p.Metadata.Name == "IsActive" || 
                            p.Metadata.Name == "IsAssigned" || 
                            p.Metadata.Name == "IsEnabled");

                        var isSoftDelete = softDeleteProperty != null &&
                                           softDeleteProperty.OriginalValue is bool original &&
                                           softDeleteProperty.CurrentValue is bool current &&
                                           original && !current;

                        if (isSoftDelete)
                        {
                            entry.Entity.DeactivatedAt = DateTime.Now;

                            entry.Property(p => p.CreatedAt).IsModified = false;
                            entry.Property(p => p.CreatedBy).IsModified = false;
                            entry.Property(p => p.UpdatedAt).IsModified = false;
                            entry.Property(p => p.UpdatedBy).IsModified = false;
                        }
                        else
                        {
                            entry.Entity.UpdatedAt = DateTime.Now;

                            entry.Property(p => p.CreatedAt).IsModified = false;
                            entry.Property(p => p.CreatedBy).IsModified = false;
                            entry.Property(p => p.DeactivatedAt).IsModified = false;
                            entry.Property(p => p.DeactivatedBy).IsModified = false;
                        }
                        break;

                }
            }
        }
    }
}

