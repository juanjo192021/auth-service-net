using Authentication.RefreshToken.Domain.Constants;
using Authentication.RefreshToken.Domain.Entities;

namespace Authentication.RefreshToken.Persistence.Seed.Data
{
    public class UserSeed
    {
        public static User SystemAdmin(string email, string password) =>
        new User
        {
            FirstName = Users.FirstName,
            LastName = Users.LastName,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            IsActive = true,
            IsBlocked = false,
        };
    }
}
