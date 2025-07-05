using ProjectFlow.Core.Extension;

namespace ProjectFlow.Core.Models.Entities;

public class ProjectService
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Hours { get; set; }
    public decimal Value { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now.ToUnspecifiedKind();
    public DateTime UpdatedAt { get; set; }
    public Project Project { get; set; } = null!;
}