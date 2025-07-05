using ProjectFlow.Core.Models.DTOs.AppSettings.Request;
using ProjectFlow.Core.Models.DTOs.AppSettings.Response;
using ProjectFlow.Core.Models.Entities;

namespace ProjectFlow.Core.Extension.Mapper;

public static class AppSettingsProfile
{
    public static AppSettings ToAppSettings(this AppSettingsRequest request)
    {
        return new AppSettings
        {
            ValuePerHour = request.ValuePerHour,
            DaysToAddOnFinishedDate = request.DaysToAddOnFinishedDate
        };
    }

    public static AppSettingsResponse ToAppSettingsResponse(this AppSettings appSettings)
    {
        return new AppSettingsResponse
        {
            ValuePerHour = appSettings.ValuePerHour,
            DaysToAddOnFinishedDate = appSettings.DaysToAddOnFinishedDate
        };
    }

    public static AppSettingsRequest ToAppSettingsRequest(this AppSettingsResponse appSettings)
    {
        return new AppSettingsRequest
        {
            ValuePerHour = appSettings.ValuePerHour,
            DaysToAddOnFinishedDate = appSettings.DaysToAddOnFinishedDate
        };
    }
}