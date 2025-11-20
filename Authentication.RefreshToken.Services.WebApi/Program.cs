using Asp.Versioning.ApiExplorer;
using Authentication.RefreshToken.Application.UseCases;
using Authentication.RefreshToken.Infrastructure;
using Authentication.RefreshToken.Persistence;
using Authentication.RefreshToken.Persistence.Seed;
using Authentication.RefreshToken.Services.WebApi.Modules.Cors;
using Authentication.RefreshToken.Services.WebApi.Modules.GlobalException;
using Authentication.RefreshToken.Services.WebApi.Modules.Middleware;
using Authentication.RefreshToken.Services.WebApi.Modules.Swagger;
using Authentication.RefreshToken.Services.WebApi.Modules.Versioning;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
}); ;

// Dependencies Injection
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();

// Custom Modules
builder.Services.AddVersioning();
builder.Services.AddSwagger();
builder.Services.AddCors(builder.Configuration);
builder.Services.AddTransient<GlobalExceptionMiddleware>();

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
            cfg.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                              description.GroupName.ToUpperInvariant());
        }

        cfg.RoutePrefix = ""; // Prefix for accessing Swagger UI
        cfg.DisplayRequestDuration();
        cfg.EnableDeepLinking();
        cfg.ShowExtensions();
    });
}

app.UseHttpsRedirection();
app.UseCors("policyApiEcommerce");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.AddMiddleware();

app.Run();
