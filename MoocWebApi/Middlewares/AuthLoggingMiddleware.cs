using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MoocWebApi.Middlewares
{
    public class AuthLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthLoggingMiddleware> _logger;

        public AuthLoggingMiddleware(RequestDelegate next, ILogger<AuthLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("=== Auth Logging Start ===");
            _logger.LogInformation($"Request Path: {context.Request.Path}");
            _logger.LogInformation($"Request Method: {context.Request.Method}");
            
            // Authentication Type
            _logger.LogInformation($"Auth Type: {context.User?.Identity?.AuthenticationType ?? "none"}");
            _logger.LogInformation($"Is Authenticated: {context.User?.Identity?.IsAuthenticated ?? false}");
            
            // Authorization Header
            if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                _logger.LogInformation($"Authorization Header: {authHeader}");
            }
            else
            {
                _logger.LogInformation("No Authorization Header Found");
            }

            // Claims information
            if (context.User?.Identity?.IsAuthenticated ?? false)
            {
                _logger.LogInformation("User Claims:");
                foreach (var claim in context.User.Claims)
                {
                    _logger.LogInformation($"  {claim.Type}: {claim.Value}");
                }
            }
            
            _logger.LogInformation("=== Auth Logging End ===");

            await _next(context);
        }
    }
}
