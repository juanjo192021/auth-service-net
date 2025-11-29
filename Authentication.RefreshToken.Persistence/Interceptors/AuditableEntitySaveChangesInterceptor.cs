using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Domain.Common;
using Authentication.RefreshToken.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Authentication.RefreshToken.Persistence.Interceptors
{
    public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService;
        public AuditableEntitySaveChangesInterceptor(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }
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
                        entry.Entity.CreatedBy = _currentUserService.UserId ?? Users.Id;
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        if (!entry.Properties.Any(p => p.IsModified))
                            continue;

                        var softDeleteProperty = entry.Properties
                            .FirstOrDefault(p =>
                                p.Metadata.Name == "IsActive" ||
                                p.Metadata.Name == "IsAssigned" ||
                                p.Metadata.Name == "IsEnabled");

                        bool isSoftDelete = false;
                        if (softDeleteProperty != null)
                        {
                            var original = softDeleteProperty.OriginalValue as bool?;
                            var current = softDeleteProperty.CurrentValue as bool?;
                            isSoftDelete = original == true && current == false;
                        }

                        if (isSoftDelete)
                        {
                            entry.Entity.DeactivatedBy = _currentUserService.UserId;
                            entry.Entity.DeactivatedAt = DateTime.UtcNow;
                            //entry.Property(p => p.CreatedAt).IsModified = false;
                            //entry.Property(p => p.CreatedBy).IsModified = false;
                            //entry.Property(p => p.UpdatedAt).IsModified = false;
                            //entry.Property(p => p.UpdatedBy).IsModified = false;
                        }
                        else
                        {
                            entry.Entity.UpdatedBy = _currentUserService.UserId;
                            entry.Entity.UpdatedAt = DateTime.UtcNow;
                            //entry.Property(p => p.CreatedAt).IsModified = false;
                            //entry.Property(p => p.CreatedBy).IsModified = false;
                            //entry.Property(p => p.DeactivatedAt).IsModified = false;
                            //entry.Property(p => p.DeactivatedBy).IsModified = false;
                        }
                        break;
                }
            }
        }
    }
}

