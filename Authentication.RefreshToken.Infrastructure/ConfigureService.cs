using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Infrastructure.Security;
using Authentication.RefreshToken.Infrastructure.Security.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Authentication.RefreshToken.Infrastructure
{
    public static class ConfigureService
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();

            var key = Encoding.UTF8.GetBytes(jwtSettings!.Key);

            services.AddHttpContextAccessor(); // Se agrego para poder usar HttpContext en JwtService

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero
                };

                /*options.Events = new JwtBearerEvents
                {
                    // ❗ Token inválido o manipulado
                    OnAuthenticationFailed = async context =>
                    {
                        context.NoResult();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var error = new ErrorResponse
                        {
                            Type = ErrorTypeUris.Unauthorized,
                            Title = "Token inválido",
                            Status = StatusCodes.Status401Unauthorized,
                            Errors = context.Exception.Message,
                            TraceId = context.HttpContext.TraceIdentifier
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(error));
                    },

                    // ❗ Token faltante o no válido
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var error = new ErrorResponse
                        {
                            Type = ErrorTypeUris.Unauthorized,
                            Title = "No autorizado",
                            Status = StatusCodes.Status401Unauthorized,
                            Errors = "Token inválido, expirado o faltante. Envíe Authorization: Bearer <token>.",
                            TraceId = context.HttpContext.TraceIdentifier,
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(error));
                    },

                    // ❗ Rol o política insuficiente
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";

                        var error = new ErrorResponse
                        {
                            Type = ErrorTypeUris.Forbidden,
                            Title = "Acceso denegado",
                            Status = StatusCodes.Status403Forbidden,
                            Errors = "No tienes permisos para acceder a este recurso.",
                            TraceId = context.HttpContext.TraceIdentifier,
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(error));
                    },

                    
                    // 🔧 OPCIONAL: si algún día extraes token desde cookies o query string
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["access_token"];
                        if (!string.IsNullOrEmpty(token))
                            context.Token = token;

                        return Task.CompletedTask;
                    }
                    
                };*/
            });

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
