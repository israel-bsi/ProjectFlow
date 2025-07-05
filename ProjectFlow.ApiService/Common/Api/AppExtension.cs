using Microsoft.EntityFrameworkCore;
using ProjectFlow.ApiService.Data;
using ProjectFlow.ApiService.Services;
using ProjectFlow.Core.Services;
using ProjectFlow.ServiceDefaults;

namespace ProjectFlow.ApiService.Common.Api;

public static class AppExtension
{
    public static void ConfigureLogging(this WebApplication app)
    {
        var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
        if (!Directory.Exists(logDirectory))
            Directory.CreateDirectory(logDirectory);

        var syncLoggerProvider = new LoggerProvider(logDirectory);
        app.Logger.LogInformation("Adicionando LoggerProvider");
        app.Services.GetRequiredService<ILoggerFactory>().AddProvider(syncLoggerProvider);
    }

    public static void ApplyDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }

    public static void ConfigureEnvironment(this WebApplication app)
    {
        if (app.Environment.IsProduction())
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return;
        }

        app.UseSwagger();
        app.MapSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjectFlow API v1");
            c.RoutePrefix = string.Empty;
        });
        app.UseDeveloperExceptionPage();
    }

    public static void UseSecurity(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    public static void ConfigureEndpoints(this WebApplication app)
    {
        app.MapDefaultEndpoints();
        app.UseHttpsRedirection();
        app.MapControllers();
    }
}