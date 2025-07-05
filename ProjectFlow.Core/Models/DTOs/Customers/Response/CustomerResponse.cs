using ProjectFlow.Core.Enums;

namespace ProjectFlow.Core.Models.DTOs.Customers.Response;

public class CustomerResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string CellPhone { get; set; } = string.Empty;
    public EPersonType PersonType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}