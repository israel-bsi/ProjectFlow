using ProjectFlow.Core.Models.DTOs.Developers.Request;
using ProjectFlow.Core.Models.DTOs.Developers.Response;
using ProjectFlow.Core.Models.Entities;

namespace ProjectFlow.Core.Extension.Mapper;

public static class DeveloperProfile
{
    public static Developer ToDeveloper(this DeveloperRequest developer)
    {
        return new Developer
        {
            Id = developer.Id,
            Name = developer.Name,
            DevLevel = developer.DevLevel,
            Technologies = developer.Technologies,
            ExpirienceTime = developer.ExpirienceTime,
        };
    }

    public static DeveloperResponse ToDeveloperResponse(this Developer developer)
    {
        return new DeveloperResponse
        {
            Id = developer.Id,
            Name = developer.Name,
            DevLevel = developer.DevLevel,
            Technologies = developer.Technologies,
            ExpirienceTime = developer.ExpirienceTime,
            CreatedAt = developer.CreatedAt,
            UpdatedAt = developer.UpdatedAt
        };
    }

    public static DeveloperRequest ToDeveloperRequest(this DeveloperResponse developer)
    {
        return new DeveloperRequest
        {
            Id = developer.Id,
            Name = developer.Name,
            DevLevel = developer.DevLevel,
            Technologies = developer.Technologies,
            ExpirienceTime = developer.ExpirienceTime,
        };
    }

    public static void UpdateEntity(this Developer developer, DeveloperRequest request)
    {
        developer.Name = request.Name;
        developer.DevLevel = request.DevLevel;
        developer.Technologies = request.Technologies;
        developer.ExpirienceTime = request.ExpirienceTime;
        developer.UpdatedAt = DateTime.Now.ToUnspecifiedKind();
    }
}