using ProjectFlow.Core.Enums;

namespace ProjectFlow.Core.Models.DTOs.Projects.Request;

public class UpdatePaymentStatusProjectRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public EPaymentStatus PaymentStatus { get; set; }
}