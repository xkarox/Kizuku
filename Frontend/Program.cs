using Blazing.Mvvm;
using Core.Entities;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Frontend;
using Frontend.Cookie;
using Frontend.Pages.StudyManagement.Pages.ModuleDetails;
using Frontend.Services;
using Frontend.Services.Interfaces;
using Frontend.StateContainers;
using Frontend.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using Sysinfocus.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMvvm(options => {
    options.HostingModelType = BlazorHostingModelType.WebAssembly;
});


// add state container
builder.Services.AddScoped<AuthComponentStateContainer>();

// add ui stuff 
builder.Services.AddSysinfocus(jsCssFromCDN: false);
var vmAssembly = typeof(AuthViewModel).Assembly;
var vmAssembly2 = typeof(ModuleDetailsViewModel).Assembly;
builder.Services.AddMvvm(options => {
    options.RegisterViewModelsFromAssembly(vmAssembly);
    options.RegisterViewModelsFromAssembly(vmAssembly2);
});

// add auth stuff 
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<CookieDelegatingHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, 
    CookieAuthenticationStateProvider>();

// add services 
builder.Services.AddScoped<IAuthenticationServiceUI, 
    AuthenticationServiceUi>();
builder.Services.AddScoped<IStudyManagementServiceUI, StudyManagementServiceUi>();

var baseUrl = builder.Configuration.GetValue<string>("ApiBaseUrl");
if (string.IsNullOrEmpty(baseUrl))
    throw new ApplicationException("Missing API BaseUrl");
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(baseUrl);
})
.AddHttpMessageHandler<CookieDelegatingHandler>();

builder.Logging.AddConfiguration(builder.Configuration
    .GetSection("Logging"));
    

await builder.Build().RunAsync();