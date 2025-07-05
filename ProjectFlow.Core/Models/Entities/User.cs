using Microsoft.AspNetCore.Identity;

namespace ProjectFlow.Core.Models.Entities;

public class User : IdentityUser<int>
{
    public string GivenName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<Project>? Projects { get; set; } = new HashSet<Project>();
}