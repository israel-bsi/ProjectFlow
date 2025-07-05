using System.ComponentModel.DataAnnotations;

namespace ProjectFlow.Core.Enums;

public enum EProjectStatus
{
    [Display(Name = "Análise")]
    Analysis = 1,
    [Display(Name = "Desenvolvimento")]
    Development = 2,
    [Display(Name = "Validação")]
    Validation = 3,
    [Display(Name = "Concluído")]
    Finished = 4
}