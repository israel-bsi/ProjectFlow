using System.ComponentModel.DataAnnotations;

namespace ProjectFlow.Core.Enums;

public enum EDevLevel
{
    [Display(Name = "Estagiário")]
    Intern = 0,
    [Display(Name = "Trainee")]
    Trainee = 1,
    [Display(Name = "Júnior")]
    Junior = 2,
    [Display(Name = "Pleno")]
    Mid = 3,
    [Display(Name = "Sênior")]
    Senior = 4
}