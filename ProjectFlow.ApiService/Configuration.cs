namespace ProjectFlow.ApiService;

public static class Configuration
{
    public const string CorsPolicyName = "projectflow";
    public static SecretsConfiguration Secrets { get; set; } = new();
    public static EmailConfiguration EmailSettings { get; set; } = new();
    public static BudgetsSettings Budgets { get; set; } = new();

    public class SecretsConfiguration
    {
        public string JwtPrivateKey { get; set; } = "D+S3[tXj52v1660Z1W5ybXou6c1jL^]S8!PP|4t2=?0*zE2uQ`^LAl1~K0K";
    }
    public class EmailConfiguration
    {
        public string EmailFrom { get; set; } = string.Empty;
        public string EmailPassword { get; set; } = string.Empty;
    }

    public class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

    public class BudgetsSettings
    {
        public string Path { get; set; } = string.Empty;
    }
}