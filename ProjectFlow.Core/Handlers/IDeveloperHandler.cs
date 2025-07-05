using ProjectFlow.Core.Models.DTOs.Developers.Request;
using ProjectFlow.Core.Models.DTOs.Developers.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.Core.Handlers;

public interface IDeveloperHandler
{
    Task<Response<DeveloperResponse>> CreateAsync(DeveloperRequest request);
    Task<Response<DeveloperResponse>> UpdateAsync(DeveloperRequest request);
    Task<Response<DeveloperResponse>> DeleteAsync(DeleteDeveloperRequest request);
    Task<Response<DeveloperResponse>> GetByIdAsync(GetDeveloperByIdRequest request);
    Task<PagedResponse<List<DeveloperResponse>>> GetAllAsync(GetAllDevelopersRequest request);
}