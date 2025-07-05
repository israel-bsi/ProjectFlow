using ProjectFlow.ServiceDefaults;
using ProjectFlow.Web.Extensions;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");
builder.Services.AddMvc();
builder.Services.AddProgressiveWebApp();
builder.AddLogging();
builder.AddServices();
builder.AddHttpClient();
builder.AddServiceDefaults();
builder.WebHost.UseIISIntegration();
builder.Services.AddRazorComponents(options =>
{
    options.DetailedErrors = true;
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();