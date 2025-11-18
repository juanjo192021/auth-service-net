using Mapster;
using System.Reflection;

namespace Authentication.RefreshToken.Application.UseCases.Common.Mappings
{
    public static class MapsterConfiguration
    {
        public static void RegisterMappings()
        {
            var config = TypeAdapterConfig.GlobalSettings;
            // 📦 Escanea automáticamente todas las clases que implementan IRegister
            config.Scan(Assembly.GetExecutingAssembly());
        }
    }
}
