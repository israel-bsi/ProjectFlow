using ProjectFlow.Core.Enums;
using ProjectFlow.Core.Extension.Mapper;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.AppSettings.Request;
using ProjectFlow.Core.Models.DTOs.Filter;
using ProjectFlow.Core.Models.DTOs.Projects.Request;
using ProjectFlow.Web.Components.Projects;
using ProjectFlow.Web.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ProjectFlow.Web.Pages.Projects;

public class ListProjectsPage : ComponentBase
{
    #region Properties

    public MudDataGrid<ProjectRequest> DataGrid { get; set; } = null!;
    public List<ProjectRequest> Projects { get; set; } = [];
    public string SearchTerm { get; set; } = string.Empty;
    public FilterOption SelectedFilter { get; set; } = new();
    public bool ShowMudSelectStatus { get; set; }
    public bool ShowMudSelectPaymentStatus { get; set; }
    public ProjectRequest InputModel { get; set; } = new();
    public AppSettingsRequest AppSettingsRequest { get; set; } = new();

    public readonly List<FilterOption> FilterOptions =
    [
        new() { DisplayName = "Id", PropertyName = "Id" },
        new() { DisplayName = "Título", PropertyName = "Title" },
        new() { DisplayName = "Pedido", PropertyName = "Order" },
        new() { DisplayName = "Solicitante", PropertyName = "Requester" },
        new() { DisplayName = "Cliente", PropertyName = "Customer.Name" },
        new() { DisplayName = "Desenvolvedor", PropertyName = "Developers.Name" },
        new() { DisplayName = "Status", PropertyName = "ProjectStatus", Type = typeof(EProjectStatus) },
        new() { DisplayName = "Pagamento", PropertyName = "PaymentStatus", Type = typeof(EPaymentStatus) }
    ];

    #endregion

    #region Services

    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IProjectHandler Handler { get; set; } = null!;
    [Inject] public IAppSettingsHandler AppSettingsHandler { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;
    [Inject] public ProjectServices ProjectServices { get; set; } = null!;

    #endregion

    #region Methods

    public async Task<GridData<ProjectRequest>> LoadServerData(GridState<ProjectRequest> state)
    {
        await LoadAppSettingsAsync();
        var request = new GetAllProjectsRequest
        {
            PageNumber = state.Page + 1,
            PageSize = state.PageSize,
            SearchTerm = SearchTerm,
            FilterBy = SelectedFilter.PropertyName
        };

        var response = await Handler.GetAllAsync(request);
        if (response.IsSuccess)
        {
            Projects = response.Data?.Select(c => c.ToProjectRequest()).ToList() ?? [];
            return new GridData<ProjectRequest>
            {
                Items = Projects,
                TotalItems = response.TotalCount
            };
        }

        Snackbar.Add(response.Message ?? string.Empty, Severity.Error);
        return new GridData<ProjectRequest>();
    }

    public async Task OnDeleteButtonClickedAsync(ProjectRequest project)
    {
        var result = await ProjectServices.DeleteProjectAsync(project);
        if (result.IsSuccess)
        {
            Projects.RemoveAll(x => x.Id == project.Id);
            await DataGrid.ReloadServerData();
            Snackbar.Add($"Projeto {project.Title} excluído", Severity.Success);
        }
    }

    public void OnButtonSearchClick() => DataGrid.ReloadServerData();

    public void OnClearSearchClick()
    {
        SearchTerm = string.Empty;
        SelectedFilter = new FilterOption();
        ShowMudSelectStatus = false;
        ShowMudSelectPaymentStatus = false;
        DataGrid.ReloadServerData();
    }

    public void OnValueFilterChanged(FilterOption newValue)
    {
        SelectedFilter = newValue;

        if (SelectedFilter.Type == typeof(EProjectStatus))
        {
            ShowMudSelectStatus = true;
            ShowMudSelectPaymentStatus = false;
            SearchTerm = ((int)InputModel.ProjectStatus).ToString();
        }
        else if (SelectedFilter.Type == typeof(EPaymentStatus))
        {
            ShowMudSelectPaymentStatus = true;
            ShowMudSelectStatus = false;
            SearchTerm = ((int)InputModel.ProjectStatus).ToString();
        }
    }

    public void OnMudSelectStatusChanged(EProjectStatus newValue)
    {
        InputModel.ProjectStatus = newValue;
        SearchTerm = ((int)newValue).ToString();
    }

    public void OnMudSelectPaymentStatusChanged(EPaymentStatus newValue)
    {
        InputModel.PaymentStatus = newValue;
        SearchTerm = ((int)newValue).ToString();
    }

    public string GetDevelopersName(ProjectRequest project)
        => string.Join(", ", project.Developers.Select(x => x.Name));

    public async Task SelectProject(ProjectRequest project)
    {
        var parameters = new DialogParameters
        {
            { "InputModel", project}
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Large,
            FullWidth = true, 
            CloseOnEscapeKey = true
        };
        var dialog = await DialogService.ShowAsync<ProjectDetails>(null, parameters, options);
        var result = await dialog.Result;
        if (result?.Data is true)
            await DataGrid.ReloadServerData();
    }

    private async Task LoadAppSettingsAsync()
    {
        try
        {
            var appSettingsResponse = await AppSettingsHandler.GetAppSettingsAsync();
            if (appSettingsResponse.IsSuccess)
                AppSettingsRequest = appSettingsResponse.Data?.ToAppSettingsRequest() ?? new AppSettingsRequest();
            else
                Snackbar.Add($"Erro ao carregar configurações do aplicativo: {appSettingsResponse.Message}", Severity.Error);
        }
        catch (Exception e)
        {
            Snackbar.Add($"Erro ao carregar a página: {e.Message}", Severity.Error);
        }
    }
    #endregion
}