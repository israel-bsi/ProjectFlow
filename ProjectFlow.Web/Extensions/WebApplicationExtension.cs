using System.Globalization;
using Blazored.LocalStorage;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Web.Handler;
using ProjectFlow.Web.Security;
using ProjectFlow.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using MudBlazor.Translations;

namespace ProjectFlow.Web.Extensions;

public static class WebApplicationExtension
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CurrentUICulture;

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddMudServices();
        builder.Services.AddMudTranslations();
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
        builder.Services.AddScoped(x => (IJwtAuthenticationStateProvider)x.GetRequiredService<AuthenticationStateProvider>());
        builder.Services.AddTransient<IAccountHandler, AccountHandler>();
        builder.Services.AddTransient<IDeveloperHandler, DeveloperHandler>();
        builder.Services.AddTransient<ICustomerHandler, CustomerHandler>();
        builder.Services.AddTransient<IProjectHandler, ProjectHandler>();
        builder.Services.AddTransient<IAppSettingsHandler, AppSettingsHandler>();
        builder.Services.AddTransient<ProjectServices>();
        builder.Services.AddLocalization();
    }

    public static void AddHttpClient(this WebApplicationBuilder builder)
    {
        string url;
        if (builder.Environment.IsProduction())
        {
            var scope = builder.Services.BuildServiceProvider().CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            url = configuration["API_URL"] ?? "";
        }
        else
        {
            url = Core.Configuration.BackendUrl;
        }
        Configuration.BackendUrl = url;
        builder.Services.AddHttpClient(Configuration.HttpClientName, options =>
        {
            options.BaseAddress = new(url);
            options.Timeout = TimeSpan.FromMinutes(10);
        });
    }

    public static void AddLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        var date = DateTime.Now.ToString("yyyy-MM-dd");
        var logPath = Path.Combine("logs", $"{date}.txt");
        var logDirectory = Path.GetDirectoryName(logPath);
        if (!Directory.Exists(logDirectory))
            Directory.CreateDirectory(logDirectory!);
        builder.Logging.AddFile(logPath);
        builder.Logging.AddConsole();
    }
}