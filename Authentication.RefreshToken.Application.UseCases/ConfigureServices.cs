using Authentication.RefreshToken.Application.UseCases.Common.Behaviours;
using Authentication.RefreshToken.Application.UseCases.Common.Mappings;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Authentication.RefreshToken.Application.UseCases
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Mapster Configuration
            MapsterConfiguration.RegisterMappings();
            var config = TypeAdapterConfig.GlobalSettings;
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            // FluentValidation Configuration
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // MediatR Configuration
            services.AddMediatR(
                cfg =>
                {
                    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); // Registers all IRequestHandler, INotificationHandler, etc.
                    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>)); // Registers the Validation Behaviour
                });

            return services;
        }
    }
}
