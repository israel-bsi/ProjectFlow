namespace ProjectFlow.Core.Request.Emails;

public class ResetPasswordMessage : EmailMessage
{
    public string ResetPasswordLink { get; set; } = string.Empty;
}