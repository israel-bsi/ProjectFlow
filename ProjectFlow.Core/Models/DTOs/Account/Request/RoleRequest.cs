using System.ComponentModel.DataAnnotations;

namespace ProjectFlow.Core.Models.DTOs.Account.Request;

public class RoleRequest
{
    [Required(ErrorMessage = "Campo Email é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Campo RoleName é obrigatório")]
    public string RoleName { get; set; } = string.Empty;
}