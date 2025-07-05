using ProjectFlow.Core.Models.DTOs.Account.Response;
using ProjectFlow.Core.Request;

namespace ProjectFlow.Core.Models.DTOs.Developers.Request;

public class DeleteDeveloperRequest : EntityIdRequest
{
    public UserResponse User { get; set; } = new();
}