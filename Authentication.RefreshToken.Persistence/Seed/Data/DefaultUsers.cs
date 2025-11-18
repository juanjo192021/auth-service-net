using Authentication.RefreshToken.Domain.Entities;

namespace Authentication.RefreshToken.Persistence.Seed.Data
{
    public class DefaultUsers
    {
        public static User SystemAdmin(string email, string password) =>
        new User
        {
            FirstName = "System",
            LastName = "Administrator",
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            IsActive = true,
            IsBlocked = false,
        };
    }
}
