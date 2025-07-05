using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Services;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next,
        ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ocorreu um erro durante o procesamento da requisição");
            await HandleExceptionAsync(context, e);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var result = JsonSerializer.Serialize(new ErrorData
        {
            Description = "Erro interno no servidor",
            Errors = new List<Error>
            {
                new()
                {
                    Field = "Exception",
                    Message = exception.Message
                }
            }
        }, options: new() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return context.Response.WriteAsync(result);
    }
}