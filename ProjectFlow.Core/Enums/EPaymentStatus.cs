using System.ComponentModel.DataAnnotations;

namespace ProjectFlow.Core.Enums;

public enum EPaymentStatus
{
    [Display(Name = "Pendente")]
    Peding = 1,
    [Display(Name = "Parcial")]
    Partial = 2,
    [Display(Name = "Total")]
    Total = 3
}