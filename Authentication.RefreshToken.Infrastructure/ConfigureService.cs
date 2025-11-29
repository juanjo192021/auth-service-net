using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Concerns.Common;
using Authentication.RefreshToken.Infrastructure.Security;
using Authentication.RefreshToken.Infrastructure.Security.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

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

                options.Events = new JwtBearerEvents
                {
                    // Token inválido o manipulado
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                        //context.NoResult();
                        //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        //context.Response.ContentType = "application/json";

                        //var error = new ErrorResponse
                        //{
                        //    Message = "Token invalid for authentication",
                        //};

                        //await context.Response.WriteAsync(JsonSerializer.Serialize(error));
                    },

                    // Token faltante o no válido
                    OnChallenge = context =>
                    {
                        //context.HandleResponse();

                        //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        //context.Response.ContentType = "application/json";

                        //var error = new ErrorResponse
                        //{
                        //    Message = "Token inválido, expirado o faltante. Envíe Authorization: Bearer <token>.",
                        //};

                        //await context.Response.WriteAsync(JsonSerializer.Serialize(error));
                        // ⚠️ No uses HandleResponse aquí o rompes el refresh
                        return Task.CompletedTask;
                    },

                    // Rol o política insuficiente
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";

                        var error = new ErrorResponse("No tienes permisos para acceder a este recurso.");

                        await context.Response.WriteAsync(JsonSerializer.Serialize(error));
                    },


                    // si algún día extraes token desde cookies o query string
                    //OnMessageReceived = context =>
                    //{
                    //    var token = context.Request.Cookies["access_token"];
                    //    if (!string.IsNullOrEmpty(token))
                    //        context.Token = token;

                    //    return Task.CompletedTask;
                    //}
                };
            });

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
