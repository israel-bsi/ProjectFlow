using System.ComponentModel.DataAnnotations;

namespace ProjectFlow.Core.Models.DTOs.Account.Request;

public class RegisterRequest
{
    [Required(ErrorMessage = "Campo Nome é obrigatório")]
    public string GivenName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Campo Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Campo Senha é obrigatório")]
    public string Password { get; set; } = string.Empty;
    [Compare("Password", ErrorMessage = "Senhas não coincidem")]
    public string ConfirmPassword { get; set; } = string.Empty;
}