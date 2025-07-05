using System.ComponentModel.DataAnnotations;

namespace ProjectFlow.Core.Models.DTOs.Account.Request;

public class ForgotPasswordRequest
{
    [Required(ErrorMessage = "Campo Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;
}