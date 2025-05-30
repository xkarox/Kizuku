using System.Net.Http.Json;
using System.Security.Claims;
using Core.Responses;
using Microsoft.AspNetCore.Components.Authorization;

namespace Frontend.Cookie;

public class CookieAuthenticationStateProvider(
    IHttpClientFactory httpClientFactory,
    ILogger<CookieAuthenticationStateProvider> logger) : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("API");
    private readonly ClaimsPrincipal _anonymous = new (new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/authenticate/currentUserInfo");
            if (!response.IsSuccessStatusCode)
            {
                logger.Log(LogLevel.Debug, $"Auth endpoint returned {(int)response.StatusCode}");
                return new AuthenticationState(_anonymous);
            }
            logger.Log(LogLevel.Debug, $"Auth endpoint returned {(int)response.StatusCode}");
            
            var userInfo = await response.Content.ReadFromJsonAsync<UserInfoResponse>();
            logger.Log(LogLevel.Debug, $"UserInfo: {userInfo}");
            
            if (userInfo != null && userInfo.IsAuthenticated)
            {
                var claims = userInfo.Claims.Select(c => new Claim(c.Type, c.Value)).ToList();
                var identity = new ClaimsIdentity(claims, "CookieAuth");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            logger.Log(LogLevel.Debug, "User is not authenticated or session expired");
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Debug, $"Unexpected error occured: {ex.Message}");
        }

        return new AuthenticationState(_anonymous);
    }
    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}