namespace ProjectFlow.Core.Models.DTOs.Account.Response;

public class UserResponse
{
    public int Id { get; set; }
    public string GivenName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IList<string> Roles { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}