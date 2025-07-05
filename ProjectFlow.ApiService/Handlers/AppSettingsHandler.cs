using Microsoft.EntityFrameworkCore;
using ProjectFlow.ApiService.Data;
using ProjectFlow.Core.Extension.Mapper;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.AppSettings.Request;
using ProjectFlow.Core.Models.DTOs.AppSettings.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Handlers;

public class AppSettingsHandler : IAppSettingsHandler
{
    private readonly AppDbContext _context;

    public AppSettingsHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Response<AppSettingsResponse>> UpsertAppSettingsAsync(AppSettingsRequest request)
    {
        var appSettings = await _context
            .AppSettings
            .FirstOrDefaultAsync();

        AppSettingsResponse? response;

        if (appSettings is null)
        {
            var newAppSettings = request.ToAppSettings();
            await _context.AppSettings.AddAsync(newAppSettings);
            await _context.SaveChangesAsync();
            response = newAppSettings.ToAppSettingsResponse();
            return new Response<AppSettingsResponse>(response);
        }

        appSettings.ValuePerHour = request.ValuePerHour;
        appSettings.DaysToAddOnFinishedDate = request.DaysToAddOnFinishedDate;

        _context.AppSettings.Update(appSettings);
        await _context.SaveChangesAsync();
        response = appSettings.ToAppSettingsResponse();
        return new Response<AppSettingsResponse>(response);
    }

    public async Task<Response<AppSettingsResponse>> GetAppSettingsAsync()
    {
        var appSettings = await _context
            .AppSettings
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (appSettings is null)
            return new Response<AppSettingsResponse>(null, 404,
                "Configurações do aplicativo não encontradas");

        var response = appSettings.ToAppSettingsResponse();

        return new Response<AppSettingsResponse>(response);
    }
}