using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectFlow.ApiService.Data;
using ProjectFlow.ApiService.Handlers;
using ProjectFlow.ApiService.Services;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.Entities;
using ProjectFlow.Core.Services;
using ProjectFlow.ServiceDefaults;

namespace ProjectFlow.ApiService.Common.Api;

public static class BuilderExtension
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        var basePath = builder.Environment.ContentRootPath;

        Configuration.Budgets.Path = Path.Combine(basePath, "Sources", "Budget");

        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
        builder.Services.AddControllers();
        builder.Services.AddProblemDetails();
        builder.Services.AddTransient<IAccountHandler, AccountHandler>();
        builder.Services.AddTransient<ICustomerHandler, CustomerHandler>();
        builder.Services.AddTransient<IDeveloperHandler, DeveloperHandler>();
        builder.Services.AddTransient<IProjectHandler, ProjectHandler>();
        builder.Services.AddTransient<IAppSettingsHandler, AppSettingsHandler>();
        builder.Services.AddTransient<IEmailService, EmailService>();
        builder.Services.AddTransient<TokenService>();
    }

    public static void AddDbContexts(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();

            options.LogTo(Console.WriteLine);
        });
    }

    public static void AddSecurity(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.Secrets.JwtPrivateKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        builder.Services.AddAuthorization(x =>
        {
            x.AddPolicy(Configuration.Roles.Admin, p => p.RequireRole(Configuration.Roles.Admin));
            x.AddPolicy(Configuration.Roles.User, p => p.RequireRole(Configuration.Roles.User));
        });

        builder.Services.AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 1;
            })
            .AddSignInManager()
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<AppDbContext>();
    }

    public static void AddCrossOrigin(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(
            options => options.AddPolicy(
                Configuration.CorsPolicyName,
                policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("Content-Disposition")
            ));
    }

    public static void AddDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.CustomSchemaIds(n => n.FullName);
        });
    }
}