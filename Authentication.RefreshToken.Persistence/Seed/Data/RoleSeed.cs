using Authentication.RefreshToken.Domain.Entities;
using Authentication.RefreshToken.Domain.Constants;

namespace Authentication.RefreshToken.Persistence.Seed.Data
{
    public static class RoleSeed
    {
        public static readonly Role SuperAdmin = new Role
        {
            Name = Roles.SuperAdmin,
            Description = "Full access to the entire system",
            IsActive = true
        };

        public static readonly Role Admin = new Role
        {
            Name = Roles.Admin,
            Description = "Can manage system resources except role assignments",
            IsActive = true
        };

        public static readonly Role Moderator = new Role
        {
            Name = Roles.Moderator,
            Description = "Can moderate content or users",
            IsActive = true
        };

        public static readonly Role BasicUser = new Role
        {
            Name = Roles.BasicUser,
            Description = "Default role for new users",
            IsActive = true
        };

        public static IEnumerable<Role> List => new[] { SuperAdmin, Admin, Moderator, BasicUser };
    }
}
