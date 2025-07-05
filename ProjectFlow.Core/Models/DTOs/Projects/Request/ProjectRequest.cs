using System.ComponentModel.DataAnnotations;
using ProjectFlow.Core.Enums;
using ProjectFlow.Core.Extension;
using ProjectFlow.Core.Models.DTOs.Account.Response;
using ProjectFlow.Core.Models.DTOs.Customers.Request;
using ProjectFlow.Core.Models.DTOs.Developers.Request;

namespace ProjectFlow.Core.Models.DTOs.Projects.Request;

public class ProjectRequest
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Título é obrigatório.")]
    [StringLength(80, ErrorMessage = "Título deve ter no máximo {1} caracteres.")]
    public string Title { get; set; } = string.Empty;
    [Required(ErrorMessage = "Descrição é obrigatório.")]
    public string Description { get; set; } = string.Empty;
    [Range(1, 999999999, ErrorMessage = "Horas deve ser maior que {1}.")]
    public int TotalHours { get; set; }
    [Required(ErrorMessage = "Solicitante é obrigatório.")]
    [StringLength(80, ErrorMessage = "Solicitante deve ter no máximo {1} caracteres.")]
    public string Requester { get; set; } = string.Empty;
    public decimal TotalValue { get; set; }
    public decimal DiscountValue { get; set; }
    public int DaysToAddToDeadline { get; set; }
    public int Deadline { get; set; }
    public EDiscountType DiscountType { get; set; }
    public List<ProjectServiceRequest> ProjectServices { get; set; } = [];
    public EProjectStatus ProjectStatus { get; set; } = EProjectStatus.Analysis;
    public EPaymentStatus PaymentStatus { get; set; } = EPaymentStatus.Peding;
    [Required(ErrorMessage = "Data de solicitação inválida")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? RequestedAt { get; set; } = DateTime.Now.ToUnspecifiedKind();
    public DateTime? DevelopmentStart { get; set; }
    public List<DeveloperRequest> Developers { get; set; } = [];
    public UserResponse User { get; set; } = new();
    public CustomerRequest Customer { get; set; } = new();
}