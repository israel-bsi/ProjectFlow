using ProjectFlow.Core.Handlers;
using Microsoft.AspNetCore.Components;

namespace ProjectFlow.Web.Pages.Authentication;

public partial class LogoutPage : ComponentBase
{
    #region Services

    [Inject] public IAccountHandler Handler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await Handler.LogoutAsync();
    }

    #endregion
}