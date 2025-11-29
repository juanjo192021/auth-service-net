namespace Authentication.RefreshToken.Application.UseCases.Authentication.Common
{
    internal static class UserRoleHelper
    {
        public static List<string> ExtractUserRoles(Domain.Entities.User user)
        {
            return user.UserRoles?
                .Select(ur => ur.Role.Name)
                .ToList()
                ?? new List<string>();
        }
    }
}
