using ProjectFlow.Core.Models.DTOs.Projects.Request;
using ProjectFlow.Core.Models.DTOs.Projects.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.Core.Handlers;

public interface IProjectHandler
{
    Task<Response<ProjectResponse>> CreateAsync(ProjectRequest request);
    Task<Response<ProjectResponse>> UpdateAsync(ProjectRequest request);
    Task<Response<ProjectResponse>> DeleteAsync(DeleteProjectRequest request);
    Task<Response<ProjectResponse>> GetByIdAsync(GetProjectByIdRequest request);
    Task<PagedResponse<List<ProjectResponse>>> GetAllAsync(GetAllProjectsRequest request);
    Task<Response<string>> UpdateStatusAsync(UpdateStatusProjectRequest request);
    Task<Response<string>> UpdatePaymentStatusAsync(UpdatePaymentStatusProjectRequest request);
    Task<Response<ProjectCommissionResponse>> GetCommissionsAsync(GetProjectByIdRequest request);
    Task<Response<ProjectBudgetResponse>> GetBudgetByProjectAsync(GetBudgetByProjectRequest request);
}