using ProjectFlow.Core.Models.DTOs.Account.Response;
using ProjectFlow.Core.Models.Entities;

namespace ProjectFlow.Core.Extension.Mapper;

public static class UserProfile
{
    public static UserResponse ToUserResponse(this User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            GivenName = user.GivenName,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            Roles = new List<string>()
        };
    }
}