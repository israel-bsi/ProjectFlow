namespace ProjectFlow.Core.Models.DTOs.Projects.Response;

public class ProjectCommissionResponse
{
    public decimal TotalProject { get; set; }
    public decimal TotalCommission { get; set; }
    public int ProjectId { get; set; }
    public List<CommissionResponse> Commission { get; set; } = [];
}

public class CommissionResponse
{
    public string DeveloperName { get; set; } = string.Empty;
    public decimal CommissionValue { get; set; }
}