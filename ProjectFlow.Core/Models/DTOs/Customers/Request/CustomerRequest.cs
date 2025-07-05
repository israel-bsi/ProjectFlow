using System.ComponentModel.DataAnnotations;
using ProjectFlow.Core.Enums;

namespace ProjectFlow.Core.Models.DTOs.Customers.Request;

public class CustomerRequest
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Campo Nome é obrigatório")]
    public string Name { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string CellPhone { get; set; } = string.Empty;
    public EPersonType PersonType { get; set; } = EPersonType.PhysicalPerson;
}