using ProjectFlow.Core.Extension.Mapper;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.AppSettings.Request;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ProjectFlow.Web.Pages.Settings;

public class SettingsPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; }
    public AppSettingsRequest AppSettings { get; set; } = new();

    #endregion

    #region Services

    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IAppSettingsHandler AppSettingsHandler { get; set; } = null!;

    #endregion

    #region Methods

    public async Task OnValidSubmitAppSettingsAsync()
    {
        IsBusy = true;
        try
        {
            var result = await AppSettingsHandler.UpsertAppSettingsAsync(AppSettings);
            if (result.IsSuccess)
                Snackbar.Add("Configurações salvas com sucesso", Severity.Success);
            else
                Snackbar.Add(result.Message ?? string.Empty, Severity.Error);
        }
        catch
        {
            Snackbar.Add("Erro ao salvar as configurações", Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task LoadAppSettings()
    {
        var appSettingsResponse = await AppSettingsHandler.GetAppSettingsAsync();
        if (appSettingsResponse.IsSuccess)
        {
            var result = appSettingsResponse.Data?.ToAppSettingsRequest();
            AppSettings = result ?? new AppSettingsRequest();
        }
        else
            Snackbar.Add(appSettingsResponse.Message ?? string.Empty, Severity.Error);
    }

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            await LoadAppSettings();
        }
        catch
        {
            Snackbar.Add("Erro ao carregar as configurações", Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion
}