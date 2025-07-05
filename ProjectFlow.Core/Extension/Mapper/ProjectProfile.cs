using ProjectFlow.Core.Models.DTOs.Projects.Request;
using ProjectFlow.Core.Models.DTOs.Projects.Response;
using ProjectFlow.Core.Models.Entities;

namespace ProjectFlow.Core.Extension.Mapper;

public static class ProjectProfile
{
    public static Project ToProject(this ProjectRequest request)
    {
        return new Project
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            TotalHours = request.TotalHours,
            Requester = request.Requester,
            TotalValue = request.TotalValue,
            DiscountValue = request.DiscountValue,
            DaysToAddToDeadline = request.DaysToAddToDeadline,
            Deadline = request.Deadline,
            DiscountType = request.DiscountType,
            ProjectStatus = request.ProjectStatus,
            PaymentStatus = request.PaymentStatus,
            RequestedAt = request.RequestedAt ?? DateTime.UtcNow,
            DevelopmentStart = request.DevelopmentStart,
            Developers = request.Developers.Select(d => d.ToDeveloper()).ToList(),
            ProjectServices = request.ProjectServices.Select(ps => ps.ToProjectService()).ToList(),
        };
    }

    public static ProjectResponse ToProjectResponse(this Project project)
    {
        return new ProjectResponse
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            TotalHours = project.TotalHours,
            Requester = project.Requester,
            TotalValue = project.TotalValue,
            DiscountValue = project.DiscountValue,
            DaysToAddToDeadline = project.DaysToAddToDeadline,
            Deadline = project.Deadline,
            DiscountType = project.DiscountType,
            ProjectStatus = project.ProjectStatus,
            PaymentStatus = project.PaymentStatus,
            RequestedAt = project.RequestedAt,
            DevelopmentStart = project.DevelopmentStart,
            DevelopmentEnd = project.DevelopmentEnd,
            ValidationStart = project.ValidationStart,
            ValidationEnd = project.ValidationEnd,
            FinishedIn = project.FinishedIn,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            Developers = project.Developers.Select(d => d.ToDeveloperResponse()).ToList(),
            ProjectServices = project.ProjectServices.Select(ps => ps.ToProjectServiceResponse()).ToList(),
            Customer = project.Customer.ToCustomerResponse(),
            User = project.User.ToUserResponse()
        };
    }

    public static ProjectRequest ToProjectRequest(this ProjectResponse project)
    {
        return new ProjectRequest
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            TotalHours = project.TotalHours,
            Requester = project.Requester,
            TotalValue = project.TotalValue,
            DiscountValue = project.DiscountValue,
            DaysToAddToDeadline = project.DaysToAddToDeadline,
            Deadline = project.Deadline,
            DiscountType = project.DiscountType,
            ProjectStatus = project.ProjectStatus,
            PaymentStatus = project.PaymentStatus,
            RequestedAt = project.RequestedAt,
            DevelopmentStart = project.DevelopmentStart,
            Customer = project.Customer.ToCustomerRequest(),
            Developers = project.Developers.Select(d => d.ToDeveloperRequest()).ToList(),
            ProjectServices = project.ProjectServices.Select(ps => ps.ToProjectServiceRequest()).ToList(),
        };
    }
}