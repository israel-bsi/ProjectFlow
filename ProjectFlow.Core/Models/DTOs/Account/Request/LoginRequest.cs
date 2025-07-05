using System.ComponentModel.DataAnnotations;

namespace ProjectFlow.Core.Models.DTOs.Account.Request;

public class LoginRequest
{
    [Required(ErrorMessage = "Campo Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Campo Senha é obrigatório")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}