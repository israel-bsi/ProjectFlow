using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace ProjectFlow.Web.Extensions;

public static class HttpClientExtension
{
    public static async Task AddJwtBearer(this HttpClient httpClient, ILocalStorageService localStorage)
    {
        var token = await localStorage.GetItemAsync<string>(Configuration.TokenName);
        if (!string.IsNullOrEmpty(token))
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}