using Authentication.RefreshToken.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.RefreshToken.Persistence.Extensions
{
    public static class PermissionQueryExtensions
    {
        public static IQueryable<Permission> ApplyFullIncludes(this IQueryable<Permission> query)
        {
            return query
                //.Include(r => r.UserRoles!)
                //  .ThenInclude(ur => ur.User)
                //      .ThenInclude(u => u.CreatedByUser)
                //.Include(r => r.UserRoles!)
                //    .ThenInclude(ur => ur.User.UpdateByUser)
                //.Include(r => r.UserRoles!)
                //    .ThenInclude(ur => ur.User.DeactivatedByUser)

                .Include(r => r.RolePermissions!)
                    .ThenInclude(rp => rp.Role)
                        .ThenInclude(p => p.CreatedByUser)
                .Include(r => r.RolePermissions!)
                    .ThenInclude(rp => rp.Role.UpdateByUser)
                .Include(r => r.RolePermissions!)
                    .ThenInclude(rp => rp.Role.DeactivatedByUser)

                .Include(r => r.CreatedByUser)
                .Include(r => r.UpdateByUser)
                .Include(r => r.DeactivatedByUser);

        }
    }
}
