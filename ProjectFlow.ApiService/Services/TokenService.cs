using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ProjectFlow.Core.Models.DTOs.Account.Response;
using ProjectFlow.Core.Models.Entities;

namespace ProjectFlow.ApiService.Services;

public class TokenService
{
    private readonly UserManager<User> _userManager;

    public TokenService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<TokenResponse>  Create(User user)
    {
        var handler = new JwtSecurityTokenHandler();

        var bytes = Encoding.UTF8.GetBytes(Configuration.Secrets.JwtPrivateKey);

        var key = new SymmetricSecurityKey(bytes);

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claimsIdentity = await GenerateClaimsAsync(user);

        var tokenDescriptior = new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Expires = DateTime.Now.AddHours(10),
            Subject = claimsIdentity
        };

        var token = handler.CreateToken(tokenDescriptior);

        var tokenString = handler.WriteToken(token);

        return new TokenResponse
        {
            AccessToken = tokenString,
            TokenType = "Bearer",
            ExpiresIn = 36000
        };
    }

    private async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var ci = new ClaimsIdentity();

        ci.AddClaim(new Claim("Id", user.Id.ToString()));
        ci.AddClaim(new Claim(ClaimTypes.Name, user.Email!));
        ci.AddClaim(new Claim(ClaimTypes.Email, user.Email!));
        ci.AddClaim(new Claim(ClaimTypes.GivenName, user.GivenName));

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
            ci.AddClaim(new Claim(ClaimTypes.Role, role));

        return ci;
    }
}