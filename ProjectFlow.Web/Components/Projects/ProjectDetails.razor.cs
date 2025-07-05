using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.AppSettings.Request;
using ProjectFlow.Core.Models.DTOs.AppSettings.Response;
using ProjectFlow.Core.Models.DTOs.Projects.Request;
using ProjectFlow.Core.Models.DTOs.Projects.Response;
using ProjectFlow.Web.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ProjectFlow.Web.Components.Projects;

public class ProjectDetailsComponent : ComponentBase
{
    public ProjectCommissionResponse CommissionResponse { get; set; } = new();

    [Parameter]
    public ProjectRequest InputModel { get; set; } = new();

    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; } = null!;

    public AppSettingsResponse AppSettings { get; set; } = new();

    [Inject]
    public ProjectServices ProjectServices { get; set; } = null!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject] public IProjectHandler ProjectHandler { get; set; } = null!;

    [Inject] public IAppSettingsHandler AppSettingsHandler { get; set; } = null!;

    public async Task OnDeleteButtonClicked()
    {
        var result = await ProjectServices.DeleteProjectAsync(InputModel);
        if (result.IsSuccess)
        {
            Snackbar.Add($"Projeto {InputModel.Title} excluído", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var request = new GetProjectByIdRequest { Id = InputModel.Id };
            var response  = await ProjectHandler.GetCommissionsAsync(request);
            if (response.IsSuccess)
                CommissionResponse = response.Data ?? new ProjectCommissionResponse();
            else
                Snackbar.Add($"Erro ao carregar comissões: {response.Message}", Severity.Error);

            var appSettingsResponse = await AppSettingsHandler.GetAppSettingsAsync();
            if (appSettingsResponse.IsSuccess)
                AppSettings = appSettingsResponse.Data ?? new AppSettingsResponse();
            else
                Snackbar.Add($"Erro ao carregar configurações do aplicativo: {appSettingsResponse.Message}", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Erro ao carregar detalhes do projeto: {ex.Message}", Severity.Error);
        }
    }
}