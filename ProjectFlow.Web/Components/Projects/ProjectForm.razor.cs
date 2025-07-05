using ProjectFlow.Core.Enums;
using ProjectFlow.Core.Extension.Mapper;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Customers.Request;
using ProjectFlow.Core.Models.DTOs.Developers.Request;
using ProjectFlow.Core.Models.DTOs.Projects.Request;
using ProjectFlow.Core.Models.DTOs.Projects.Response;
using ProjectFlow.Core.Response;
using ProjectFlow.Web.Components.Customers;
using ProjectFlow.Web.Components.Developers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ProjectFlow.Web.Components.Projects;

public class ProjectFormComponent : ComponentBase
{
    #region Parameters

    [Parameter] public int Id { get; set; }

    #endregion

    #region Properties

    public string Operation => Id != 0 ? "Editar" : "Cadastrar";
    public bool IsBusy { get; set; }
    public ProjectRequest InputModel { get; set; } = new();
    public decimal ValueHour { get; set; }
    public bool Discount { get; set; }

    #endregion

    #region Services

    [Inject] public IAppSettingsHandler SettingsHandler { get; set; } = null!;
    [Inject] public IProjectHandler ProjectHandler { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        try
        {
            if (InputModel.Developers.Count == 0)
            {
                Snackbar.Add("Adicione ao menos um desenvolvedor", Severity.Error);
                IsBusy = false;
                return;
            }

            Response<ProjectResponse> result;
            if (InputModel.Id > 0)
                result = await ProjectHandler.UpdateAsync(InputModel);
            else
                result = await ProjectHandler.CreateAsync(InputModel);

            if (result.IsSuccess)
            {
                var operation = InputModel.Id > 0 ? "atualizado" : "cadastrado";
                Snackbar.Add($"Projeto {operation} com sucesso", Severity.Success);
                NavigationManager.NavigateTo("/projetos");
            }
            else
                Snackbar.Add(result.Message ?? string.Empty, Severity.Error);
        }
        catch
        {
            Snackbar.Add($"Falha ao {Operation.ToLower()} projeto.", Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task OpenCustomerDialog()
    {
        var parameters = new DialogParameters
        {
            { "OnCustomerSelected", EventCallback.Factory
                .Create<CustomerRequest>(this, SelectedCustomer) }
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Large,
            FullWidth = true
        };
        var dialog = await DialogService
            .ShowAsync<CustomerDialog>("Informe o cliente", parameters, options);
        await dialog.Result;
    }
    private void SelectedCustomer(CustomerRequest customer)
    {
        InputModel.Customer = customer;
        StateHasChanged();
    }

    public async Task OpenDevelopersDialog()
    {
        var parameters = new DialogParameters
        {
            { "OnDevelopersSelected", EventCallback.Factory
                .Create<HashSet<DeveloperRequest>>(this, SelectedDevelopers) }
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true
        };
        var dialog = await DialogService
            .ShowAsync<DeveloperDialog>("Informe os desenvoledores", parameters, options);
        await dialog.Result;
    }
    private void SelectedDevelopers(HashSet<DeveloperRequest> developers)
    {
        InputModel.Developers = developers.ToList();
        StateHasChanged();
    }

    public void AddService()
    {
        InputModel.ProjectServices.Add(new ProjectServiceRequest());
    }

    public void RefreshState(List<ProjectServiceRequest> services)
    {
        InputModel.ProjectServices = services;
        InputModel.TotalHours = services.Sum(x => x.Hours);
        InputModel.TotalValue = services.Sum(x => x.Value);
        InputModel.Deadline = (int)Math.Ceiling(InputModel.TotalHours / 2.0);
        StateHasChanged();
    }

    public void OnBlurDiscountValue()
    {
        if (InputModel.DiscountType == EDiscountType.Percentage)
        {
            var discountValue = InputModel.TotalValue * (InputModel.DiscountValue / 100);
            InputModel.TotalValue -= discountValue;
        }
        else
        {
            InputModel.TotalValue -= InputModel.DiscountValue;
        }
    }

    public void OnDiscountChecked(bool newValue)
    {
        if (newValue)
        {
            Discount = true;
            return;
        }

        Discount = false;
        InputModel.DiscountValue = 0;
        InputModel.TotalValue = InputModel.ProjectServices.Sum(x => x.Value);
    }

    public void OnDaysToAddToDeadlineChanged(int newValue)
    {
        InputModel.DaysToAddToDeadline = newValue;
        InputModel.Deadline = InputModel.TotalHours / 2 + newValue;
    }
    public DateMask _mask = new DateMask("dd-MM-yyyy");
    public void OnTimePickerChanged(DateTime? newValue)
    {
        InputModel.RequestedAt = newValue;
        StateHasChanged();
    }
    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var settingsResult = await SettingsHandler.GetAppSettingsAsync();
            if (settingsResult is { IsSuccess: true, Data: not null })
                ValueHour = settingsResult.Data.ValuePerHour;

            if (Id != 0)
            {
                GetProjectByIdRequest? request = null;
                try
                {
                    request = new GetProjectByIdRequest { Id = Id };
                }
                catch
                {
                    Snackbar.Add("Parâmetro inválido", Severity.Error);
                }

                if (request is null) return;

                var response = await ProjectHandler.GetByIdAsync(request);
                if (response is { IsSuccess: true, Data: not null })
                {
                    InputModel = response.Data.ToProjectRequest();
                    Discount = InputModel.DiscountValue > 0;
                }
                else
                    Snackbar.Add(response.Message ?? "Erro ao carregar dados", Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add("Erro ao carregar dados", Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion
}