using ProjectFlow.Core.Enums;

namespace ProjectFlow.Core.Models.DTOs.Developers.Response;

public class DeveloperResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Technologies { get; set; } = string.Empty;
    public int ExpirienceTime { get; set; }
    public EDevLevel DevLevel { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}