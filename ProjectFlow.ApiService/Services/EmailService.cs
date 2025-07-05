using System.Net;
using System.Net.Mail;
using ProjectFlow.Core.Request.Emails;
using ProjectFlow.Core.Response;
using ProjectFlow.Core.Services;

namespace ProjectFlow.ApiService.Services;

public class EmailService : IEmailService
{
    public Task<Response<bool>> SendResetPasswordLinkAsync(ResetPasswordMessage message)
    {
        throw new NotImplementedException();
    }
    private static SmtpClient ConfigureClient()
    {
        return new SmtpClient
        {
            Host = "smtp.gmail.com",
            EnableSsl = true,
            Port = 587,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(
                Configuration.EmailSettings.EmailFrom,
                Configuration.EmailSettings.EmailPassword)
        };
    }

    private static MailMessage ConfigureMailMessage()
    {
        return new MailMessage
        {
            From = new MailAddress(Configuration.EmailSettings.EmailFrom, "Realty Hub")
        };
    }
}