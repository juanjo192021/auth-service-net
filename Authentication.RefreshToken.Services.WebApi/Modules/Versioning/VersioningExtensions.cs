using Asp.Versioning;

namespace Authentication.RefreshToken.Services.WebApi.Modules.Versioning
{
    public static class VersioningExtensions
    {
        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                //options.ApiVersionReader = new HeaderApiVersionReader("x-version");
                //options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                // Reemplaza el segmento de la URL con la versión actual de la API
                options.SubstituteApiVersionInUrl = true;
            });
            return services;
        }
    }
}
