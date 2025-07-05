using ProjectFlow.Core.Models.DTOs.Account.Request;
using ProjectFlow.Core.Models.DTOs.Account.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.Core.Handlers;

public interface IAccountHandler
{
    Task<Response<TokenResponse>> LoginAsync(LoginRequest request);
    Task<Response<object>> RegisterAsync(RegisterRequest request);
    Task<Response<object>> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<Response<object>> ResetPasswordAsync(ResetPasswordRequest request);
    Task<Response<UserResponse>> GetUserInfoAsync(int id);
    Task<Response<object>> LogoutAsync();
}