using Backend.Infrastructure;
using Backend.Repositories;
using Core;
using Core.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration
                           .GetConnectionString("DatabaseConnection")
                       ?? throw new InvalidOperationException(
                           "Connection string 'DatabaseConnection' not found.");

builder.Services.AddDbContext<KizukuContext>(options =>
    options.UseSqlite($"Data Source={connectionString}"));

// Cookie Authentication
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.ExpireTimeSpan = TimeSpan.FromHours(30);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden";
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Repositories for data access
builder.Services.AddScoped<IUserRepository>();

// Services for business logic
builder.Services.AddScoped<IAuthenticationService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();