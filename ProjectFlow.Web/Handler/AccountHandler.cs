using Blazored.LocalStorage;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Account.Request;
using ProjectFlow.Core.Models.DTOs.Account.Response;
using ProjectFlow.Core.Response;
using ProjectFlow.Web.Extensions;
using ProjectFlow.Web.Security;

namespace ProjectFlow.Web.Handler;

public class AccountHandler : IAccountHandler
{
    private readonly HttpClient _httpClient;
    private readonly IJwtAuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AccountHandler(IHttpClientFactory factory,
        IJwtAuthenticationStateProvider authenticationStateProvider,
        ILocalStorageService localStorage)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
        _httpClient = factory.CreateClient(Configuration.HttpClientName);
    }

    public async Task<Response<TokenResponse>> LoginAsync(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/account/login", request);

        var result = await response.Content.ReadFromJsonAsync<Response<TokenResponse>>()
            ?? new Response<TokenResponse>(null, 400, message: "Erro ao realizar login");
        if (!result.IsSuccess)
            return new Response<TokenResponse>(null, message: result.Message);

        var token = result.Data?.AccessToken;
        if (string.IsNullOrEmpty(token))
            return new Response<TokenResponse>(null, message: "Token inválido recebido.");

        await _authenticationStateProvider.MarkUserAsAuthenticated(token);
        return new Response<TokenResponse>(result.Data, result.Code, "Login realizado com sucesso");
    }

    public async Task<Response<object>> RegisterAsync(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/account/register", request);
        
        return await response.ProcessResponseAsync<object>();
    }

    public async Task<Response<object>> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/account/forgot-password", request);

        return await response.ProcessResponseAsync<object>();
    }

    public async Task<Response<object>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("v1/account/reset-password", request);

        return await response.ProcessResponseAsync<object>();
    }

    public async Task<Response<UserResponse>> GetUserInfoAsync(int id)
    {
        await _httpClient.AddJwtBearer(_localStorage);
        var response = await _httpClient.GetAsync($"v1/account/{id}");

        return await response.ProcessResponseAsync<UserResponse>();
    }

    public async Task<Response<object>> LogoutAsync()
    {
        await _authenticationStateProvider.MarkUserAsLoggedOut();
        return new Response<object>(null, message: "Logout realizado com sucesso");
    }
}