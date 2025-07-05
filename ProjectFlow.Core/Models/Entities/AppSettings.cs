namespace ProjectFlow.Core.Models.Entities;

public class AppSettings
{
    public int Id { get; set; }
    public decimal ValuePerHour { get; set; }
    public int DaysToAddOnFinishedDate { get; set; }
}