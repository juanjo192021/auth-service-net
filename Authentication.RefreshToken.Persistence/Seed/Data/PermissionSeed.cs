using Authentication.RefreshToken.Domain.Constants;
using Authentication.RefreshToken.Domain.Entities;
using System.Reflection;

namespace Authentication.RefreshToken.Persistence.Seed.Data
{
    public static class PermissionSeed
    {
        public static IEnumerable<Permission> GetPermissionsFromConstants()
        {
            // Usamos Reflection para leer todas las constantes publicas de la clase Permissions
            var permissionConstants = typeof(Permissions)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(x => (string)x.GetValue(null)!)
                .ToList();

            foreach (var permName in permissionConstants)
            {
                yield return new Permission
                {
                    Name = permName,
                    Description = $"System permission for {permName}", // Opcional: podrías mapear descripciones reales
                    IsActive = true
                };
            }
        }
    }
}
