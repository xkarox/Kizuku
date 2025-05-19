using System.Net.Http.Json;
using System.Text.Json;
using Core;
using Core.Errors;
using Core.Requests;
using Core.Responses;
using Frontend.Cookie;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace Frontend.Services;

public class AuthenticationServiceUi(
    IHttpClientFactory httpClientFactory,
    AuthenticationStateProvider authStateProvider,
    ILogger<AuthenticationServiceUi> logger)
    : IAuthenticationServiceUI
{
    private readonly CookieAuthenticationStateProvider _authStateProvider =
        (authStateProvider as CookieAuthenticationStateProvider)!;
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("API");

    public async Task<Result<LoginResponse>> Login(LoginRequest loginRequest)
    {
        try
        {
            var response =
                await _httpClient.PostAsJsonAsync("api/Auth/login",
                    loginRequest);
            if (!response.IsSuccessStatusCode)
            {
                var error = await SafeReadFromJsonAsync<Error>(response.Content);
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
        catch (OperationCanceledException e)
        {
            logger.Log(LogLevel.Debug, "Login cancelled");
            return Result<LoginResponse>.Failure(
                new Error("No Response from Backend"));
        }
        
    }

    public async Task Logout()
    {
        try
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
                logger.Log(LogLevel.Debug,
                    $"Logout failed: {error?.Message ?? "Unknown error"}");
            }
        }
        catch (OperationCanceledException e)
        {
            logger.Log(LogLevel.Debug, "Logout cancelled");
        }
    }

    public async Task<Result<RegistrationResponse>> Register(RegistrationRequest registrationRequest)
    {
        try
        {
            var response =
                await _httpClient.PostAsJsonAsync("api/Auth/register",
                    registrationRequest);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Error>();
                logger.Log(LogLevel.Debug,
                    error != null ? error.Message : "Unknown error");
                return Result<RegistrationResponse>.Failure(error ??
                    new Error("Unknown error"));
            }

            ;
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
        catch (OperationCanceledException e)
        {
            logger.Log(LogLevel.Debug, "Registration cancelled");
            return Result<RegistrationResponse>.Failure(new Error("No Response from Backend"));
        }
    }
    
    private async Task<T?> SafeReadFromJsonAsync<T>(HttpContent content)
    {
        if (content == null || content.Headers.ContentLength == 0)
            return default;
        try
        {
            string responseText = await content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseText))
                return default;
            
            return JsonSerializer.Deserialize<T>(responseText);
        }
        catch (JsonException)
        {
            return default;
        }
    }
}