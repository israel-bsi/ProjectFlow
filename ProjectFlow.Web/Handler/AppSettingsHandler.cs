using Blazored.LocalStorage;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.AppSettings.Request;
using ProjectFlow.Core.Models.DTOs.AppSettings.Response;
using ProjectFlow.Core.Response;
using ProjectFlow.Web.Extensions;

namespace ProjectFlow.Web.Handler;

public class AppSettingsHandler : IAppSettingsHandler
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public AppSettingsHandler(IHttpClientFactory httpClientFactory,
        ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);
    }

    public async Task<Response<AppSettingsResponse>> UpsertAppSettingsAsync(AppSettingsRequest request)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var result = await _httpClient.PutAsJsonAsync("v1/appsettings", request);

        return await result.ProcessResponseAsync<AppSettingsResponse>();
    }

    public async Task<Response<AppSettingsResponse>> GetAppSettingsAsync()
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var result = await _httpClient.GetAsync("v1/appsettings");

        return await result.ProcessResponseAsync<AppSettingsResponse>();
    }
}