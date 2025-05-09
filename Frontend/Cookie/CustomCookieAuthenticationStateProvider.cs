using System.Net.Http.Json;
using System.Security.Claims;
using Core.Responses;
using Microsoft.AspNetCore.Components.Authorization;

public class CustomCookieAuthenticationStateProvider(
    IHttpClientFactory httpClientFactory) : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("API");
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userInfo = await _httpClient.GetFromJsonAsync<UserInfoResponse>("api/Auth/currentUserInfo");

            if (userInfo != null && userInfo.IsAuthenticated)
            {
                var claims = userInfo.Claims.Select(c => new Claim(c.Type, c.Value)).ToList();
                var identity = new ClaimsIdentity(claims, "CookieAuth");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Console.WriteLine("User is not authenticated or session expired");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching authentication state: {ex.Message}");
        }

        return new AuthenticationState(_anonymous);
    }
    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}