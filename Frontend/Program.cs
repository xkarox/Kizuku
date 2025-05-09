using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Frontend;
using Frontend.Cookie;
using Frontend.Services;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Sysinfocus.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddSysinfocus(jsCssFromCDN: false);
builder.Services.AddScoped<CookieDelegatingHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomCookieAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://localhost:7003");
})
.AddHttpMessageHandler<CookieDelegatingHandler>();


    

await builder.Build().RunAsync();