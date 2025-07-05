using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Common.Api;

public static class ModelStateExtension
{
    public static ErrorData CreateErrorResponse(
        this ModelStateDictionary modelState,
        object model,
        int statusCode = 400,
        string? title = null)
    {
        var context = new ValidationContext(model, serviceProvider: null, items: null);
        var results = new List<ValidationResult>();

        Validator.TryValidateObject(model, context, results, validateAllProperties: true);

        var errors = results.Select(result => new Error
        {
            Field = result.MemberNames.FirstOrDefault() ?? string.Empty,
            Message = result.ErrorMessage ?? "Erro de validação"
        }).ToList();

        return new ErrorData
        {
            HttpStatusCode = statusCode,
            Description = title ?? "Requisição inválida",
            Errors = errors.ToList()
        };
    }
}