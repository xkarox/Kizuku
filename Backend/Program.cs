using Backend.Infrastructure;
using Backend.Infrastructure.Repositories;
using Backend.Json;
using Backend.Services;
using Backend.Validators;
using Core;
using Core.Validators;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration
                           .GetConnectionString("Database")
                       ?? throw new InvalidOperationException(
                           "Connection string 'Database' not found.");

builder.Services.AddDbContext<IKizukuContext, KizukuContext>(options =>
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

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
        opts.JsonSerializerOptions
            .Converters
            .Add(new ErrorConverterFactory()));;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Kizuku API",
        Version = "v1"
    });
});



// Repositories for data access
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Services for business logic
builder.Services.AddScoped<IPasswordValidator, PasswordValidator>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kizuku API v1");
        c.RoutePrefix = "";
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();