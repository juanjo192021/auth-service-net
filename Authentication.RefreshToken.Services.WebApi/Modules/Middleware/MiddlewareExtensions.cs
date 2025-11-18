using Authentication.RefreshToken.Services.WebApi.Modules.GlobalException;

namespace Authentication.RefreshToken.Services.WebApi.Modules.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder AddMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
