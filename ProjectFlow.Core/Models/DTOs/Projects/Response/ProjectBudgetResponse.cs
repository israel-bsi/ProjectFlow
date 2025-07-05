namespace ProjectFlow.Core.Models.DTOs.Projects.Response;

public class ProjectBudgetResponse
{
    public byte[] FileContents { get; set; } = [];
    public string ContentType { get; set; } = string.Empty;
    public string FileDownloadName { get; set; } = string.Empty;
}