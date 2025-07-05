using System.ComponentModel.DataAnnotations;

namespace ProjectFlow.Core.Models.DTOs.AppSettings.Request;

public class AppSettingsRequest
{
    [Required(ErrorMessage = "Campo obrigatório")]
    public decimal ValuePerHour { get; set; }

    [Required(ErrorMessage = "Campo obrigatório")]
    public int DaysToAddOnFinishedDate { get; set; }
}