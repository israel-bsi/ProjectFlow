using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace ProjectFlow.Web.Security;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider, IJwtAuthenticationStateProvider
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly ILocalStorageService _localStorage;

    public JwtAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>(Configuration.TokenName);
        var identity = new ClaimsIdentity();

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var jwtToken = _tokenHandler.ReadJwtToken(token);

                if (jwtToken.ValidTo > DateTime.UtcNow)
                {
                    identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                }
            }
            catch
            {
                // ignored
            }
        }

        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        await _localStorage.SetItemAsync(Configuration.TokenName, token);

        var jwtToken = _tokenHandler.ReadJwtToken(token);
        var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _localStorage.RemoveItemAsync(Configuration.TokenName);

        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void NotifyAuthenticationStateChanged() 
        => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
}