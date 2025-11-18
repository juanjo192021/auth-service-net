using Authentication.RefreshToken.Domain.Entities;

namespace Authentication.RefreshToken.Persistence.Seed.Data
{
    public static class DefaultRoles
    {
        public static readonly Role SuperAdmin = new Role
        {
            Name = "SUPER_ADMIN",
            Description = "Full access to the entire system",
            IsActive = true
        };

        public static readonly Role Admin = new Role
        {
            Name = "ADMIN",
            Description = "Can manage system resources except role assignments",
            IsActive = true
        };

        public static readonly Role Moderator = new Role
        {
            Name = "MODERATOR",
            Description = "Can moderate content or users",
            IsActive = true
        };

        public static readonly Role BasicUser = new Role
        {
            Name = "BASIC_USER",
            Description = "Default role for new users",
            IsActive = true
        };

        public static IEnumerable<Role> List => new[] { SuperAdmin, Admin, Moderator, BasicUser };
    }
}
