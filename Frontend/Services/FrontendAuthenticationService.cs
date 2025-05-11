using System.Net.Http.Json;
using Core;
using Core.Errors;
using Core.Requests;
using Core.Responses;
using Frontend.Cookie;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace Frontend.Services;

public class FrontendAuthenticationService(
    IHttpClientFactory httpClientFactory,
    AuthenticationStateProvider authStateProvider,
    ILogger<FrontendAuthenticationService> logger)
    : IFrontendAuthenticationService
{
    private readonly CookieAuthenticationStateProvider _authStateProvider =
        (authStateProvider as CookieAuthenticationStateProvider)!;
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("API");

    public async Task<Result<LoginResponse>> Login(LoginRequest loginRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/login", loginRequest);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>();
            logger.Log(LogLevel.Debug, error != null ? error.Message : "Unknown error");
            return Result<LoginResponse>.Failure(error ?? new Error("No error provided"));
        }
        _authStateProvider.NotifyAuthenticationStateChanged();
        var loginResponse =  await response.Content.ReadFromJsonAsync<LoginResponse>();
        
        if (loginResponse == null)
        {
            logger.Log(LogLevel.Debug, "Login failed unexpectedly");
            return Result<LoginResponse>.Failure(
                new Error("No LoginResponse received"));
        }
        logger.Log(LogLevel.Debug, loginResponse.ToString());
        return Result<LoginResponse>.Success(loginResponse);
    }

    public async Task Logout()
    {
        var response = await _httpClient.PostAsync("api/Auth/logout", null);
        if (response.IsSuccessStatusCode)
        {
            logger.Log(LogLevel.Debug, "Logout successful");
            _authStateProvider.NotifyAuthenticationStateChanged();
        }
        else
        {
            var error = await response.Content.ReadFromJsonAsync<Error>();
            logger.Log(LogLevel.Warning, $"Logout failed: {error?.Message ?? "Unknown error"}");
        }
    }

    public async Task<Result<RegistrationResponse>> Register(RegistrationRequest registrationRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/register", registrationRequest);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>();
            logger.Log(LogLevel.Debug, error != null ? error.Message : "Unknown error");
            return Result<RegistrationResponse>.Failure(error ?? new Error("Unknown error"));
        };
        var registrationResponse = await response.Content
            .ReadFromJsonAsync<RegistrationResponse>();
        if (registrationResponse == null)
        {
            logger.Log(LogLevel.Debug, "Registration failed unexpectedly");
            return Result<RegistrationResponse>.Failure(
                new Error("No RegistrationResponse received"));
        }
        logger.Log(LogLevel.Debug, registrationResponse.ToString());
        return Result<RegistrationResponse>.Success(registrationResponse);
    }
}