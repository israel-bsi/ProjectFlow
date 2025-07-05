using Blazored.LocalStorage;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Projects.Request;
using ProjectFlow.Core.Models.DTOs.Projects.Response;
using ProjectFlow.Core.Response;
using ProjectFlow.Web.Extensions;

namespace ProjectFlow.Web.Handler;

public class ProjectHandler : IProjectHandler
{
    private readonly HttpClient _httpClient;

    private readonly ILocalStorageService _localStorage;

    public ProjectHandler(IHttpClientFactory httpClientFactory,
        ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _httpClient = httpClientFactory
            .CreateClient(Configuration.HttpClientName);
    }

    public async Task<Response<ProjectResponse>> CreateAsync(ProjectRequest request)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var result = await _httpClient.PostAsJsonAsync("v1/projects", request);

        return await result.ProcessResponseAsync<ProjectResponse>();
    }

    public async Task<Response<ProjectResponse>> UpdateAsync(ProjectRequest request)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var result = await _httpClient.PutAsJsonAsync($"v1/projects/{request.Id}", request);

        return await result.ProcessResponseAsync<ProjectResponse>();
    }

    public async Task<Response<ProjectResponse>> DeleteAsync(DeleteProjectRequest request)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var result = await _httpClient.DeleteAsync($"v1/projects/{request.Id}");

        return await result.ProcessResponseAsync<ProjectResponse>();
    }

    public async Task<Response<ProjectResponse>> GetByIdAsync(GetProjectByIdRequest request)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var result = await _httpClient.GetAsync($"v1/projects/{request.Id}");

        return await result.ProcessResponseAsync<ProjectResponse>();
    }

    public async Task<PagedResponse<List<ProjectResponse>>> GetAllAsync(GetAllProjectsRequest request)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var url = $"v1/projects?pageNumber={request.PageNumber}&pageSize={request.PageSize}";

        if (!string.IsNullOrEmpty(request.FilterBy))
            url = $"{url}&filterBy={request.FilterBy}";

        if (!string.IsNullOrEmpty(request.SearchTerm))
            url = $"{url}&searchTerm={request.SearchTerm}";

        var result = await _httpClient.GetAsync(url);

        return await result.ProcessPagedResponseAsync<List<ProjectResponse>>();
    }

    public async Task<Response<string>> UpdateStatusAsync(UpdateStatusProjectRequest request)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var result = await _httpClient.PutAsJsonAsync($"v1/projects/{request.Id}/status", request);

        return await result.ProcessResponseAsync<string>();
    }

    public async Task<Response<string>> UpdatePaymentStatusAsync(UpdatePaymentStatusProjectRequest request)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var result = await _httpClient.PutAsJsonAsync($"v1/projects/{request.Id}/paymentstatus", request);

        return await result.ProcessResponseAsync<string>();
    }

    public async Task<Response<ProjectCommissionResponse>> GetCommissionsAsync(GetProjectByIdRequest request)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var result = await _httpClient.GetAsync($"v1/projects/{request.Id}/commission");

        return await result.ProcessResponseAsync<ProjectCommissionResponse>();
    }

    public async Task<Response<ProjectBudgetResponse>> GetBudgetByProjectAsync(GetBudgetByProjectRequest request)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var result = await _httpClient.GetAsync($"v1/projects/{request.Id}/budget");

        if (!result.IsSuccessStatusCode) return await result.ProcessResponseAsync<ProjectBudgetResponse>();

        var contentDisposition = result.Content.Headers.ContentDisposition;
        var fileName = contentDisposition?.FileNameStar ?? contentDisposition?.FileName ?? "Orçamento";
        var content = await result.Content.ReadAsByteArrayAsync();

        return new Response<ProjectBudgetResponse>(new ProjectBudgetResponse
        {
            FileDownloadName = fileName,
            FileContents = content
        });
    }
}