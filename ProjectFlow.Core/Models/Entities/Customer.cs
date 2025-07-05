using ProjectFlow.Core.Enums;
using ProjectFlow.Core.Extension;

namespace ProjectFlow.Core.Models.Entities;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string CellPhone { get; set; } = string.Empty;
    public EPersonType PersonType { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now.ToUnspecifiedKind();
    public DateTime UpdatedAt { get; set; }
    public ICollection<Project>? Projects { get; set; } = new HashSet<Project>();
}