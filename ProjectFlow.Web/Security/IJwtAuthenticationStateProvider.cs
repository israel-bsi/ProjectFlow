using Microsoft.AspNetCore.Components.Authorization;

namespace ProjectFlow.Web.Security;

public interface IJwtAuthenticationStateProvider
{
    Task<AuthenticationState> GetAuthenticationStateAsync();
    Task MarkUserAsAuthenticated(string token);
    Task MarkUserAsLoggedOut();
    void NotifyAuthenticationStateChanged();
}