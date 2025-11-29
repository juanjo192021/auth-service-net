using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Persistence.Contexts;
using Authentication.RefreshToken.Persistence.Interceptors;
using Authentication.RefreshToken.Persistence.Repository;
using Authentication.RefreshToken.Persistence.Seed;
using Authentication.RefreshToken.Persistence.Seed.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Authentication.RefreshToken.Persistence
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Interceptors
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();

            // DbContext
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var env = sp.GetRequiredService<IHostEnvironment>();
                var interceptor = sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>();

                options.UseSqlServer(
                    configuration.GetConnectionString("AuthRefreshTokenConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));

                options.AddInterceptors(interceptor);

                if (env.IsDevelopment())
                    options.EnableSensitiveDataLogging();
            });

            // Seed Settings
            services.Configure<AdminSettings>(configuration.GetSection("DefaultAdmin"));
            services.AddScoped<DatabaseSeeder>();

            // Repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IUserRefreshTokenRepository, UserRefreshTokenRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();

            return services;
        }
    }
}
