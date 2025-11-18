using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Authentication.RefreshToken.Services.WebApi.Modules.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInformationForApiVersion(description));
            }
        }

        static OpenApiInfo CreateInformationForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Version = description.ApiVersion.ToString(),
                Title = "Authentication Services API",
                Description = "Authentication services with ASP .NET Core Web API.",
                TermsOfService = new Uri("https://juanjo-dev.netlify.app/"),
                Contact = new OpenApiContact
                {
                    Name = "Juan José Pérez",
                    Email = "juanjodev1011@gmail.com",
                    Url = new Uri("https://juanjo-dev.netlify.app/")
                },
                License = new OpenApiLicense
                {
                    Name = "Use under LICX",
                    Url = new Uri("https://juanjo-dev.netlify.app/"),
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
