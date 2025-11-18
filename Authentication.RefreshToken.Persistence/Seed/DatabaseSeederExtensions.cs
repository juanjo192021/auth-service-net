using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Authentication.RefreshToken.Persistence.Seed
{
    public static class DatabaseSeederExtensions
    {
        public static async Task UseDatabaseSeedAsync(this IHost app)
        {
            using var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();

            await seeder.SeedAsync();
        }
    }
}
