using ProjectFlow.Core.Enums;
using ProjectFlow.Core.Models.DTOs.Account.Response;
using ProjectFlow.Core.Models.DTOs.Customers.Response;
using ProjectFlow.Core.Models.DTOs.Developers.Response;

namespace ProjectFlow.Core.Models.DTOs.Projects.Response;

public class ProjectResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TotalHours { get; set; }
    public string Requester { get; set; } = string.Empty;
    public decimal TotalValue { get; set; }
    public decimal DiscountValue { get; set; }
    public EDiscountType DiscountType { get; set; }
    public int DaysToAddToDeadline { get; set; }
    public int Deadline { get; set; }
    public EProjectStatus ProjectStatus { get; set; }
    public EPaymentStatus PaymentStatus { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? DevelopmentStart { get; set; }
    public DateTime? DevelopmentEnd { get; set; }
    public DateTime? ValidationStart { get; set; }
    public DateTime? ValidationEnd { get; set; }
    public DateTime? FinishedIn { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<DeveloperResponse> Developers { get; set; } = new HashSet<DeveloperResponse>();
    public ICollection<ProjectServiceResponse> ProjectServices { get; set; } = new HashSet<ProjectServiceResponse>();
    public CustomerResponse Customer { get; set; } = null!;
    public UserResponse User { get; set; } = null!;
}