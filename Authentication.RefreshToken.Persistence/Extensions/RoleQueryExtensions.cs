using Authentication.RefreshToken.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public static class RoleQueryExtensions
{
    public static IQueryable<Role> ApplyFullIncludes(this IQueryable<Role> query)
    {
        return query
            .Include(r => r.UserRoles!)
              .ThenInclude(ur => ur.User)
                  .ThenInclude(u => u.CreatedByUser)
            .Include(r => r.UserRoles!)
                .ThenInclude(ur => ur.User.UpdateByUser)
            .Include(r => r.UserRoles!)
                .ThenInclude(ur => ur.User.DeactivatedByUser)

            .Include(r => r.RolePermissions!)
                .ThenInclude(rp => rp.Permission)
                    .ThenInclude(p => p.CreatedByUser)
            .Include(r => r.RolePermissions!)
                .ThenInclude(rp => rp.Permission.UpdateByUser)
            .Include(r => r.RolePermissions!)
                .ThenInclude(rp => rp.Permission.DeactivatedByUser)

            .Include(r => r.CreatedByUser)
            .Include(r => r.UpdateByUser)
            .Include(r => r.DeactivatedByUser);

    }
}