using System.Text.Json.Serialization;

namespace Authentication.RefreshToken.Services.WebApi.Modules.Cors
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            const string policyName = "policyApiEcommerce";

            var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

            services.AddCors(options =>
            {
                options.AddPolicy(name: policyName, builder =>
                {
                    builder.WithOrigins(allowedOrigins ?? Array.Empty<string>())
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                var enumConverter = new JsonStringEnumConverter();
                options.JsonSerializerOptions.Converters.Add(enumConverter);
            });

            return services;
        }
    }
}
