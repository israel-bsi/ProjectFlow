using OfficeOpenXml;
using System.Globalization;
using System.Text;
using ProjectFlow.ApiService;
using ProjectFlow.ApiService.Common.Api;

var builder = WebApplication.CreateBuilder(args);

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

builder.WebHost.UseIISIntegration();
builder.AddConfiguration();
builder.AddServices();
builder.AddDbContexts();
builder.AddSecurity();
builder.AddCrossOrigin();
builder.AddDocumentation();

var app = builder.Build();

app.MapGet("/", () => "OK");
app.ApplyDatabaseMigrations();
app.ConfigureLogging();
app.ConfigureEnvironment();
app.UseCors(Configuration.CorsPolicyName);
app.UseSecurity();
app.ConfigureEndpoints();

app.Run();