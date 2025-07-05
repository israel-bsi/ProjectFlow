using ProjectFlow.Core.Models.DTOs.AppSettings.Request;
using ProjectFlow.Core.Models.DTOs.AppSettings.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.Core.Handlers;

public interface IAppSettingsHandler
{
    Task<Response<AppSettingsResponse>> UpsertAppSettingsAsync(AppSettingsRequest request);
    Task<Response<AppSettingsResponse>> GetAppSettingsAsync();
}