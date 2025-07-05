using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Developers.Request;
using ProjectFlow.Core.Models.DTOs.Developers.Response;
using ProjectFlow.Core.Response;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ProjectFlow.Web.Components.Developers;

public class DeveloperFormComponent : ComponentBase
{
    #region Parameters

    [Parameter] public int Id { get; set; }

    #endregion

    #region Properties

    public string Operation => Id != 0 ? "Editar" : "Cadastrar";
    public bool IsBusy { get; set; }
    public DeveloperRequest InputModel { get; set; } = new();

    #endregion

    #region Services

    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IDeveloperHandler Handler { get; set; } = null!;

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        try
        {
            Response<DeveloperResponse> result;
            if (InputModel.Id > 0)
                result = await Handler.UpdateAsync(InputModel);
            else
                result = await Handler.CreateAsync(InputModel);

            if (result.IsSuccess)
            {
                Snackbar.Add("Desenvolvedor cadastrado com sucesso", Severity.Success);
                NavigationManager.NavigateTo("/desenvolvedores");
            }
            else
                Snackbar.Add(result.Message ?? string.Empty, Severity.Error);
        }
        catch
        {
            Snackbar.Add($"Erro ao {Operation.ToLower()} desenvoledor", Severity.Error);
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
        IsBusy = true;
        try
        {
            if (Id != 0)
            {
                GetDeveloperByIdRequest? request = null;
                try
                {
                    request = new GetDeveloperByIdRequest { Id = Id };
                }
                catch
                {
                    Snackbar.Add("Parâmetro inválido", Severity.Error);
                }

                if (request is null) return;

                var response = await Handler.GetByIdAsync(request);
                if (response is { IsSuccess: true, Data: not null })
                {
                    InputModel = new DeveloperRequest
                    {
                        Id = response.Data.Id,
                        Name = response.Data.Name,
                        DevLevel = response.Data.DevLevel,
                        Technologies = response.Data.Technologies,
                        ExpirienceTime = response.Data.ExpirienceTime
                    };
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