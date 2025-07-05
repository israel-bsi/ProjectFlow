using System.Security.Claims;

namespace ProjectFlow.Core.Extension;

public static class ClaimsPrincipalExtension
{
    public static int Id(this ClaimsPrincipal user)
        => int.Parse(user.Claims.FirstOrDefault(c => c.Type == "Id")?.Value ?? "0");

    public static string GivenName(this ClaimsPrincipal user)
        => user.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value ?? string.Empty;

    public static string Email(this ClaimsPrincipal user)
        => user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;

    public static string Role(this ClaimsPrincipal user)
        => user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;
}