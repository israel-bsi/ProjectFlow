using System.ComponentModel.DataAnnotations;

namespace ProjectFlow.Core.Enums;

public enum EPersonType
{
    [Display(Name = "Indefinido")]
    Undefined = 0,
    [Display(Name = "Pessoa Física")]
    PhysicalPerson = 1,
    [Display(Name = "Pessoa Jurídica")]
    LegalPerson = 2
}