using Blazored.LocalStorage;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Developers.Request;
using ProjectFlow.Core.Models.DTOs.Developers.Response;
using ProjectFlow.Core.Response;
using ProjectFlow.Web.Extensions;

namespace ProjectFlow.Web.Handler;

public class DeveloperHandler : IDeveloperHandler
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public DeveloperHandler(IHttpClientFactory httpClientFactory,
        ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);
    }

    public async Task<Response<DeveloperResponse>> CreateAsync(DeveloperRequest request)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var result = await _httpClient.PostAsJsonAsync("v1/developers", request);

        return await result.ProcessResponseAsync<DeveloperResponse>();
    }

    public async Task<Response<DeveloperResponse>> UpdateAsync(DeveloperRequest request)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var result = await _httpClient.PutAsJsonAsync($"v1/developers/{request.Id}", request);

        return await result.ProcessResponseAsync<DeveloperResponse>();
    }

    public async Task<Response<DeveloperResponse>> DeleteAsync(DeleteDeveloperRequest request)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var result = await _httpClient.DeleteAsync($"v1/developers/{request.Id}");

        return await result.ProcessResponseAsync<DeveloperResponse>();
    }

    public async Task<Response<DeveloperResponse>> GetByIdAsync(GetDeveloperByIdRequest request)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var result = await _httpClient.GetAsync($"v1/developers/{request.Id}");

        return await result.ProcessResponseAsync<DeveloperResponse>();
    }

    public async Task<PagedResponse<List<DeveloperResponse>>> GetAllAsync(GetAllDevelopersRequest request)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var url = $"v1/developers?pageNumber={request.PageNumber}&pageSize={request.PageSize}";

        if (!string.IsNullOrEmpty(request.SearchTerm))
            url = $"{url}&searchTerm={request.SearchTerm}";

        var result = await _httpClient.GetAsync(url);

        return await result.ProcessPagedResponseAsync<List<DeveloperResponse>>();
    }
}