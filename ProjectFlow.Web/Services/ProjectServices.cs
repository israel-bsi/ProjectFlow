using ProjectFlow.Core.Extension.Mapper;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Projects.Request;
using ProjectFlow.Core.Response;
using ProjectFlow.Web.Components.Common;
using Microsoft.JSInterop;
using MudBlazor;

namespace ProjectFlow.Web.Services;

public class ProjectServices
{
    private readonly IProjectHandler _handler;
    private readonly IDialogService _dialogService;
    private readonly IJSRuntime _jsRuntime;
    private readonly ISnackbar _snackbar;

    public ProjectServices(IProjectHandler handler,
        IDialogService dialogService,
        IJSRuntime jsRuntime,
        ISnackbar snackbar)
    {
        _handler = handler;
        _dialogService = dialogService;
        _jsRuntime = jsRuntime;
        _snackbar = snackbar;
    }

    public async Task<Response<ProjectRequest>> DeleteProjectAsync(ProjectRequest project)
    {
        var parameters = new DialogParameters
        {
            { "ContentText", $"Ao prosseguir o <b>projeto {project.Title}</b> será excluido.<br> " +
                             "Esta é uma ação irreversível! Deseja continuar?" },
            { "ButtonText", "Confirmar" },
            { "ButtonColor", Color.Error }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small
        };

        var dialog = await _dialogService.ShowAsync<DialogConfirm>("Atenção", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: true })
            return new Response<ProjectRequest>();

        var handlerResult = await _handler.DeleteAsync(new DeleteProjectRequest { Id = project.Id });
        var projectRequest = handlerResult.Data?.ToProjectRequest();
        return new Response<ProjectRequest>(projectRequest);
    }

    public async Task OnBudgetButtonClickAsync(int id)
    {
        var parameters = new DialogParameters
        {
            { "ContentText", "Deseja fazer o download do orçamento?" },
            { "ButtonText", "Confirmar" },
            { "ButtonColor", Color.Success }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small
        };

        var dialog = await _dialogService.ShowAsync<DialogConfirm>("Confirmação", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await DownloadBudgetAsync(id);
        }
    }

    private async Task DownloadBudgetAsync(int id)
    {
        try
        {
            var request = new GetBudgetByProjectRequest { Id = id };
            var response = await _handler.GetBudgetByProjectAsync(request);
            if (response is { IsSuccess: true, Data: not null })
            {
                var memoryStream = new MemoryStream(response.Data.FileContents);
                var fileName = response.Data.FileDownloadName;
                using var streamRef = new DotNetStreamReference(memoryStream);
                await _jsRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
                return;
            }

            _snackbar.Add(response.Message ?? string.Empty, Severity.Error);
        }
        catch (Exception e)
        {
            _snackbar.Add(e.Message, Severity.Error);
        }
    }
}