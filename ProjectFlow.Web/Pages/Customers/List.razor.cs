using ProjectFlow.Core.Extension.Mapper;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Customers.Request;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ProjectFlow.Web.Pages.Customers;

public class ListCustomersPage : ComponentBase
{
    #region Parameters

    [Parameter]
    public EventCallback<CustomerRequest> OnCustomerSelected { get; set; }

    [Parameter]
    public string RowStyle { get; set; } = string.Empty;

    #endregion

    #region Properties

    public MudDataGrid<CustomerRequest> DataGrid { get; set; } = null!;

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
    [Inject] public ICustomerHandler Handler { get; set; } = null!;

    #endregion

    #region Methods

    public async Task<GridData<CustomerRequest>> LoadServerData(GridState<CustomerRequest> state)
    {
        var request = new GetAllCustomersRequest
        {
            PageNumber = state.Page + 1,
            PageSize = state.PageSize,
            SearchTerm = SearchTerm
        };

        var response = await Handler.GetAllAsync(request);
        if (response.IsSuccess)
            return new GridData<CustomerRequest>
            {
                Items = response.Data?.Select(c=>c.ToCustomerRequest()).ToList() ?? [],
                TotalItems = response.TotalCount
            };

        Snackbar.Add(response.Message ?? string.Empty, Severity.Error);
        return new GridData<CustomerRequest>();
    }

    public async Task SelectCustomer(CustomerRequest customer)
    {
        if (OnCustomerSelected.HasDelegate)
            await OnCustomerSelected.InvokeAsync(customer);
    }

    #endregion
}