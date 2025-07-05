using ProjectFlow.Core.Models.DTOs.Projects.Request;
using ProjectFlow.Core.Models.DTOs.Projects.Response;
using ProjectFlow.Core.Models.Entities;

namespace ProjectFlow.Core.Extension.Mapper;

public static class ProjectServiceProfile
{
    public static ProjectService ToProjectService(this ProjectServiceRequest request)
    {
        return new ProjectService
        {
            Id = request.Id,
            ProjectId = request.ProjectId,
            Description = request.Description,
            Hours = request.Hours,
            Value = request.Value,
        };
    }

    public static ProjectServiceResponse ToProjectServiceResponse(this ProjectService projectService)
    {
        return new ProjectServiceResponse
        {
            Id = projectService.Id,
            ProjectId = projectService.ProjectId,
            Description = projectService.Description,
            Hours = projectService.Hours,
            Value = projectService.Value,
        };
    }

    public static ProjectServiceRequest ToProjectServiceRequest(this ProjectServiceResponse projectService)
    {
        return new ProjectServiceRequest
        {
            Id = projectService.Id,
            ProjectId = projectService.ProjectId,
            Description = projectService.Description,
            Hours = projectService.Hours,
            Value = projectService.Value,
        };
    }

    public static void UpdateEntity(this ProjectService projectService, ProjectServiceRequest request)
    {
        projectService.Description = request.Description;
        projectService.Hours = request.Hours;
        projectService.Value = request.Value;
        projectService.UpdatedAt = DateTime.Now.ToUnspecifiedKind();
    }
}