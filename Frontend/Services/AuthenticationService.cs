using System.Net.Http.Json;
using Core.Requests;
using Core.Responses;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace Frontend.Services;

public class AuthenticationService(
    IHttpClientFactory httpClientFactory,
    AuthenticationStateProvider authStateProvider)
    : IAuthenticationService
{
    private readonly CustomCookieAuthenticationStateProvider _authStateProvider = 
        (CustomCookieAuthenticationStateProvider)authStateProvider;
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("API");

    public async Task<LoginResponse?> Login(LoginRequest loginRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/login", loginRequest);
        if (!response.IsSuccessStatusCode) return null;
        _authStateProvider.NotifyAuthenticationStateChanged();
        return await response.Content.ReadFromJsonAsync<LoginResponse>();
    }

    public async Task Logout()
    {
        var response = await _httpClient.PostAsync("api/Auth/logout", null);
        if (response.IsSuccessStatusCode)
        {
            _authStateProvider.NotifyAuthenticationStateChanged();
        }
    }

    public async Task<RegistrationResponse?> Register(RegistrationRequest registrationRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/register", registrationRequest);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content
            .ReadFromJsonAsync<RegistrationResponse>();
    }
}