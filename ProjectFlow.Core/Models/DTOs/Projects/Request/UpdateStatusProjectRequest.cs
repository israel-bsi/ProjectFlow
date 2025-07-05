using ProjectFlow.Core.Enums;

namespace ProjectFlow.Core.Models.DTOs.Projects.Request;

public class UpdateStatusProjectRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public EProjectStatus ProjectStatus { get; set; }
}