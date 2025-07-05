using System.ComponentModel.DataAnnotations;
using ProjectFlow.Core.Enums;
using ProjectFlow.Core.Models.DTOs.Account.Response;

namespace ProjectFlow.Core.Models.DTOs.Developers.Request;

public class DeveloperRequest
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Campo obrigatório")]
    public string Name { get; set; } = string.Empty;
    public string Technologies { get; set; } = string.Empty;
    public int ExpirienceTime { get; set; }
    public EDevLevel DevLevel { get; set; }
    public UserResponse User { get; set; } = new();
}