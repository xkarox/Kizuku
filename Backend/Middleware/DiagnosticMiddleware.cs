// Create DiagnosticMiddleware.cs
using System.Security.Claims;

public class DiagnosticMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DiagnosticMiddleware> _logger;

    public DiagnosticMiddleware(RequestDelegate next, ILogger<DiagnosticMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log claims *after* authentication middleware runs
        if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
        {
            _logger.LogInformation("--- User Claims Debug (Middleware) ---");
            _logger.LogInformation($"User authenticated: {context.User.Identity.Name}");
            _logger.LogInformation($"Authentication type: {context.User.Identity.AuthenticationType}");

            var claimsIdentity = context.User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                _logger.LogInformation($"ClaimsIdentity RoleClaimType: {claimsIdentity.RoleClaimType}");
                foreach (var claim in context.User.Claims)
                {
                    _logger.LogInformation($"Claim Type: {claim.Type}, Value: {claim.Value}");
                }
                bool isInRole = context.User.IsInRole("Admin");
                _logger.LogInformation($"User.IsInRole('Admin'): {isInRole}");
            } else {
                _logger.LogWarning("User.Identity is not a ClaimsIdentity.");
            }
            _logger.LogInformation("--- End User Claims Debug (Middleware) ---");
        }

        // Call the next delegate/middleware in the pipeline
        await _next(context);
    }
}