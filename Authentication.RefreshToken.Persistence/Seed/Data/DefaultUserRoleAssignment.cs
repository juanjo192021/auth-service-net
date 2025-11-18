using Authentication.RefreshToken.Domain.Entities;

namespace Authentication.RefreshToken.Persistence.Seed.Data
{
    public static class DefaultUserRoleAssignment
    {
        public static UserRole SystemAdminRole => new UserRole
        {
            UserId = 1,
            RoleId = DefaultRoles.SuperAdmin.Id,
            IsAssigned = true
        };
    }
}
