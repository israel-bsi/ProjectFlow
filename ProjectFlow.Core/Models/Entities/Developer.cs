using System.ComponentModel.DataAnnotations;
using ProjectFlow.Core.Enums;
using ProjectFlow.Core.Extension;

namespace ProjectFlow.Core.Models.Entities;

public class Developer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [MaxLength(200, ErrorMessage = "Limite máximo de 200 caracteres")]
    public string Technologies { get; set; } = string.Empty;
    public int ExpirienceTime { get; set; }
    public EDevLevel DevLevel { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now.ToUnspecifiedKind();
    public DateTime UpdatedAt { get; set; }
    public ICollection<Project>? Projects { get; set; } = new HashSet<Project>();
}