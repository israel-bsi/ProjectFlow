using System.ComponentModel.DataAnnotations;

namespace ProjectFlow.Core.Enums;

public enum EDiscountType
{
    [Display(Name = "Nenhum")]
    None = 0,
    [Display(Name = "Porcentagem")]
    Percentage = 1,
    [Display(Name = "Valor")]
    Value = 2
}