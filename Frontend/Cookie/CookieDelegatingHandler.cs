using System.Net;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Frontend.Cookie;

public class CookieDelegatingHandler(ILogger<CookieDelegatingHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException httpEx) when (httpEx.Message.Contains("TypeError: Failed to fetch"))
        {
            logger.LogError("Failed to fetch request: {RequestUri}", request.RequestUri);
        
            if (httpEx.InnerException != null)
            {
                logger.LogError("Inner exception: {InnerExceptionMessage}", httpEx.InnerException.Message);
            }
        
            return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
            {
                ReasonPhrase = "Network request failed. Please check your connection."
            };
        }
        catch (ArgumentNullException e)
        {
            logger.LogDebug("Request was null");
        }
        catch (OperationCanceledException e)
        {
            logger.LogDebug("Request was cancelled");
        }
        return new HttpResponseMessage();
    }

}

