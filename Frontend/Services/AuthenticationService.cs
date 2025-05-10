using System.Net.Http.Json;
using Core;
using Core.Errors;
using Core.Requests;
using Core.Responses;
using Frontend.Cookie;
using Microsoft.AspNetCore.Components.Authorization;
using IAuthenticationService = Frontend.Services.Interfaces.IAuthenticationService;

namespace Frontend.Services;

public class AuthenticationService(
    IHttpClientFactory httpClientFactory,
    AuthenticationStateProvider authStateProvider,
    ILogger<AuthenticationService> logger)
    : IAuthenticationService
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
            logger.Log(LogLevel.Debug, error.Message);
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
    }

    public async Task<Result<RegistrationResponse>> Register(RegistrationRequest registrationRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/register", registrationRequest);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>();
            logger.Log(LogLevel.Debug, error.Message);
            return Result<RegistrationResponse>.Failure(error ?? new Error("No error provided"));
        };
        var registrationResponse = await response.Content
            .ReadFromJsonAsync<RegistrationResponse>();
        if (registrationResponse == null)
        {
            logger.Log(LogLevel.Debug, "Registration failed unexpectedly");
            return Result<RegistrationResponse>.Failure(
                new Error("No RegistrationResponse recieved"));
        }
        logger.Log(LogLevel.Debug, registrationResponse.ToString());
        return Result<RegistrationResponse>.Success(registrationResponse);
    }
}