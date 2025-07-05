using ProjectFlow.Core.Models.DTOs.Account.Response;
using ProjectFlow.Core.Request;

namespace ProjectFlow.Core.Models.DTOs.Projects.Request;

public class DeleteProjectRequest : EntityIdRequest
{
    public UserResponse User { get; set; } = new();
}