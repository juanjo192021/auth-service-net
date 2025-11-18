using Asp.Versioning.ApiExplorer;
using Authentication.RefreshToken.Application.UseCases;
using Authentication.RefreshToken.Infrastructure;
using Authentication.RefreshToken.Persistence;
using Authentication.RefreshToken.Persistence.Seed;
using Authentication.RefreshToken.Services.WebApi.Modules.GlobalException;
using Authentication.RefreshToken.Services.WebApi.Modules.Middleware;
using Authentication.RefreshToken.Services.WebApi.Modules.Swagger;
using Authentication.RefreshToken.Services.WebApi.Modules.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddTransient<GlobalExceptionMiddleware>();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddVersioning();
builder.Services.AddSwagger();

var app = builder.Build();

await app.UseDatabaseSeedAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(cfg =>
    {

        foreach (var description in provider.ApiVersionDescriptions)
        {
            cfg.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToString()); //
        }
        cfg.RoutePrefix = "swagger"; // Prefijo de la ruta
        cfg.DisplayRequestDuration(); // Muestra la duración de la solicitud
        cfg.EnableDeepLinking(); // Enlaces para las operaciones y tag
        cfg.ShowExtensions(); // Muestra extensiones para visualizar los campos y valores para las operaciones, parámetros y esquemas 
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.AddMiddleware();

app.Run();
