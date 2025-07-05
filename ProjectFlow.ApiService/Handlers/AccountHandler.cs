using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using ProjectFlow.ApiService.Services;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Account.Request;
using ProjectFlow.Core.Models.DTOs.Account.Response;
using ProjectFlow.Core.Models.Entities;
using ProjectFlow.Core.Request.Emails;
using ProjectFlow.Core.Response;
using ProjectFlow.Core.Services;

namespace ProjectFlow.ApiService.Handlers;

public class AccountHandler : IAccountHandler
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailService _emailService;
    private readonly TokenService _tokenService;

    public AccountHandler(UserManager<User> userManager,
        SignInManager<User> signInManager,
        IEmailService emailService,
        TokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _tokenService = tokenService;
    }

    public async Task<Response<TokenResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return new Response<TokenResponse>(null, 404, "Usuário não encontrado");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            return new Response<TokenResponse>(null, 400, "Senha inválida");

        var token = await _tokenService.Create(user);
        return new Response<TokenResponse>(token);
    }

    public async Task<Response<object>> RegisterAsync(RegisterRequest request)
    {
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            GivenName = request.GivenName
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
            return new Response<object>(null, message: "Usuário cadastrado com sucesso!");

        var errors = result.Errors.Select(e => new Error
        {
            Field = e.Code,
            Message = e.Description
        });
        var errorData = new ErrorData(400, "Erro ao cadastrar usuário", errors.ToList());
        return new Response<object>(errorData, 400);
    }

    public async Task<Response<object>> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return new Response<object>(null, 404, "Usuário não encontrado");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var resetLink = $"{Core.Configuration.BackendUrl}" +
                        $"/recuperar-senha?" +
                        $"userId={user.Id}" +
                        $"&token={encodedToken}";
        var message = new ResetPasswordMessage
        {
            EmailTo = user.Email!,
            ResetPasswordLink = resetLink
        };
        await _emailService.SendResetPasswordLinkAsync(message);
        return new Response<object>(null, message: "Verifique seu email!");
    }

    public async Task<Response<object>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
            return new Response<object>(null, 404, "Usuário não encontrado");

        var decodedBytes = WebEncoders.Base64UrlDecode(request.Token);
        var decodedToken = Encoding.UTF8.GetString(decodedBytes);

        var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.Password);

        if (result.Succeeded)
            return new Response<object>(null, message: "Senha resetada com sucesso!");

        var errors = result.Errors.Select(e => new Error
        {
            Field = e.Code,
            Message = e.Description
        });
        var errorData = new ErrorData(400, "Erro ao resetar usuário", errors.ToList());
        return new Response<object>(errorData, 400);
    }

    public async Task<Response<UserResponse>> GetUserInfoAsync(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
            return new Response<UserResponse>(null, 404, "Usuário não encontrado");

        var roles = await _userManager.GetRolesAsync(user);
        var userResponse = new UserResponse
        {
            Id = user.Id,
            GivenName = user.GivenName,
            Email = user.Email!,
            UserName = user.Email!,
            Roles = roles,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
        return new Response<UserResponse>(userResponse);
    }

    public Task<Response<object>> LogoutAsync()
    {
        return Task.FromResult(new Response<object>(null, message: "Logout realizado com sucesso"));
    }
}