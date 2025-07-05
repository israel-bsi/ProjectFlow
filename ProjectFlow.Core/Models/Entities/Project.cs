using ProjectFlow.Core.Enums;
using ProjectFlow.Core.Extension;

namespace ProjectFlow.Core.Models.Entities;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TotalHours { get; set; }
    public string Requester { get; set; } = string.Empty;
    public decimal TotalValue { get; set; }
    public decimal DiscountValue { get; set; }
    public EDiscountType DiscountType { get; set; }
    public int Deadline { get; set; }
    public int DaysToAddToDeadline { get; set; }
    public EProjectStatus ProjectStatus { get; set; }
    public EPaymentStatus PaymentStatus { get; set; }
    public DateTime RequestedAt { get; set; } = DateTime.Now.ToUnspecifiedKind();
    public DateTime? DevelopmentStart { get; set; }
    public DateTime? DevelopmentEnd { get; set; }
    public DateTime? ValidationStart { get; set; }
    public DateTime? ValidationEnd { get; set; }
    public DateTime? FinishedIn { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now.ToUnspecifiedKind();
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public Customer Customer { get; set; } = null!;
    public User User { get; set; } = null!;
    public ICollection<Developer> Developers { get; set; } = new HashSet<Developer>();
    public ICollection<ProjectService> ProjectServices { get; set; } = new HashSet<ProjectService>();
    public int UserId { get; set; }
    public int CustomerId { get; set; }
}