using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Account.Request;
using ProjectFlow.Web.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ProjectFlow.Web.Pages.Authentication;

public class LoginPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; }
    public LoginRequest InputModel { get; set; } = new();

    #endregion

    #region Service

    [Inject] public IAccountHandler Handler { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public IJwtAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        try
        {
            var result = await Handler.LoginAsync(InputModel);
            if (result.IsSuccess)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                NavigationManager.NavigateTo("/");
            }
            else
                Snackbar.Add(result.Message ?? string.Empty, Severity.Error);
        }
        catch
        {
            Snackbar.Add("Erro ao realizar login", Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity is { IsAuthenticated: true })
            NavigationManager.NavigateTo("/");
    }

    #endregion
}