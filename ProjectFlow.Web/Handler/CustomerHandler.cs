using Blazored.LocalStorage;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Customers.Request;
using ProjectFlow.Core.Models.DTOs.Customers.Response;
using ProjectFlow.Core.Response;
using ProjectFlow.Web.Extensions;

namespace ProjectFlow.Web.Handler;

public class CustomerHandler : ICustomerHandler
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public CustomerHandler(IHttpClientFactory httpClientFactory,
        ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);
    }

    public async Task<Response<CustomerResponse>> GetByIdAsync(GetCustomerByIdRequest request)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var result = await _httpClient.GetAsync($"v1/customers/{request.Id}");

        return await result.ProcessResponseAsync<CustomerResponse>();
    }

    public async Task<PagedResponse<List<CustomerResponse>>> GetAllAsync(GetAllCustomersRequest request)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var url = $"v1/customers?pageNumber={request.PageNumber}&pageSize={request.PageSize}";

        if (!string.IsNullOrEmpty(request.SearchTerm))
            url = $"{url}&searchTerm={request.SearchTerm}";

        var result = await _httpClient.GetAsync(url);

        return await result.ProcessPagedResponseAsync<List<CustomerResponse>>();
    }
}