using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SecureTodoApi.Services;

namespace SecureTodoApi.Middleware
{
    public class AuditLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public AuditLoggingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);

                // Resolve the scoped service here
                var auditLogService = context.RequestServices.GetRequiredService<IAuditLogService>();

                // Get user information
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var username = context.User.FindFirst(ClaimTypes.Name)?.Value;

                // Get request information
                var actionType = context.Request.Method;
                var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                var userAgent = context.Request.Headers["User-Agent"].ToString();
                var path = context.Request.Path.Value ?? "";
                var queryString = context.Request.QueryString.ToString();

                // Determine entity type and ID from path
                var pathParts = path.Split('/');
                var entityType = pathParts.Length > 2 ? pathParts[2] : "Unknown";
                int? entityId = null;
                if (pathParts.Length > 3 && int.TryParse(pathParts[3], out var id))
                {
                    entityId = id;
                }

                // Log the action
                await auditLogService.LogActionAsync(
                    actionType,
                    userId != null ? int.Parse(userId) : null,
                    username ?? "Anonymous",
                    ipAddress,
                    userAgent,
                    entityType,
                    entityId,
                    $"{actionType} {path}{queryString}",
                    context.Response.StatusCode < 400
                );
            }
            finally
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }
}