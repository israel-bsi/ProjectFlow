using ProjectFlow.Core.Extension.Mapper;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Developers.Request;
using ProjectFlow.Web.Components.Common;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ProjectFlow.Web.Pages.Developers;

public class ListDevelopersPage : ComponentBase
{
    #region Parameters

    [Parameter]
    public EventCallback<HashSet<DeveloperRequest>> OnDevelopersSelected { get; set; }

    [Parameter] 
    public bool MultiSelection { get; set; }

    [Parameter] 
    public bool ShowActions { get; set; } = true;

    [Parameter] 
    public bool ShowConfirmButton { get; set; }

    [Parameter] 
    public string RowStyle { get; set; } = string.Empty;

    #endregion

    #region Properties

    public List<DeveloperRequest> Developers { get; set; } = [];
    public MudDataGrid<DeveloperRequest> DataGrid { get; set; } = null!;
    public HashSet<DeveloperRequest> SelectedDevelopers { get; set; } = [];

    private string _searchTerm = string.Empty;
    public string SearchTerm
    {
        get => _searchTerm;
        set
        {
            if (_searchTerm == value) return;
            _searchTerm = value;
            _ = DataGrid.ReloadServerData();
        }
    }

    #endregion

    #region Services

    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;
    [Inject] public IDeveloperHandler Handler { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    #endregion

    #region Methods

    public async Task<GridData<DeveloperRequest>> LoadServerData(GridState<DeveloperRequest> state)
    {
        var request = new GetAllDevelopersRequest
        {
            PageNumber = state.Page + 1,
            PageSize = state.PageSize,
            SearchTerm = SearchTerm
        };

        var response = await Handler.GetAllAsync(request);
        if (response.IsSuccess)
            return new GridData<DeveloperRequest>
            {
                Items = response.Data?.Select(c=>c.ToDeveloperRequest()).ToList() ?? [],
                TotalItems = response.TotalCount
            };

        Snackbar.Add(response.Message ?? string.Empty, Severity.Error);
        return new GridData<DeveloperRequest>();
    }

    public void RedirectToEdit(DeveloperRequest developer) 
        => NavigationManager.NavigateTo($"/desenvolvedores/editar/{developer.Id}");

    public async Task OnDeleteButtonClickedAsync(DeveloperRequest developer)
    {
        var parameters = new DialogParameters
        {
            { "ContentText", $"Ao prosseguir o desenvolvedor {developer.Name} será excluido.<br> " +
                             "Esta é uma ação irreversível! Deseja continuar?" },
            { "ButtonText", "Confirmar" },
            { "ButtonColor", Color.Error }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small
        };

        var dialog = await DialogService.ShowAsync<DialogConfirm>("Atenção", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: true }) return;

        await OnDeleteAsync(developer);
        StateHasChanged();
    }

    private async Task OnDeleteAsync(DeveloperRequest developer)
    {
        await Handler.DeleteAsync(new DeleteDeveloperRequest { Id = developer.Id });
        Developers.RemoveAll(x => x.Id == developer.Id);
        await DataGrid.ReloadServerData();
        Snackbar.Add($"Desenvolvedor {developer.Name} excluído", Severity.Success);
    }

    public void SelectDevelopers(HashSet<DeveloperRequest> developers) 
        => SelectedDevelopers = developers;

    public async Task SendSelectedDevelopers()
    {
        if (OnDevelopersSelected.HasDelegate)
            await OnDevelopersSelected.InvokeAsync(SelectedDevelopers);
    }

    #endregion
}