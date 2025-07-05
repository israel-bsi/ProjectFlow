using ProjectFlow.Core.Request.Emails;
using ProjectFlow.Core.Response;

namespace ProjectFlow.Core.Services;

public interface IEmailService
{
    Task<Response<bool>> SendResetPasswordLinkAsync(ResetPasswordMessage message);
}