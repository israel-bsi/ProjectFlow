using System.ComponentModel.DataAnnotations;

namespace ProjectFlow.Core.Models.DTOs.Account.Request;

public class ResetPasswordRequest
{
    [Required(ErrorMessage = "Campo Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Campo Senha é obrigatório")]
    public string Password { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}