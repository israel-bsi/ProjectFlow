using System.ComponentModel.DataAnnotations;

namespace ProjectFlow.Core.Models.DTOs.Projects.Request;

public class ProjectServiceRequest
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    [Required(ErrorMessage = "Descrição é obrigatório.")]
    public string Description { get; set; } = string.Empty;
    [Range(1, 999999999, ErrorMessage = "Horas deve ser maior que {1}.")]
    public int Hours { get; set; }
    public decimal Value { get; set; }
}