using BMS.Core.Models;
using System.Security.Claims;

namespace BMS.API.Middlewares
{
    public class ApiBehaviorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ApiBehaviorMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger("ApiBehaviorLogger");
        }

        public async Task Invoke(HttpContext context)
        {
            var startTime = DateTime.UtcNow;

            var user = context.User;
            var username = context.User?.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                ?? "Anonymous";

            var method = context.Request.Method;
            var url = context.Request.Path + context.Request.QueryString;

            context.Request.EnableBuffering();
            string body = "";

            using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
            {
                body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            if (body.Contains("password", StringComparison.OrdinalIgnoreCase))
            {
                body = "***filtered***";
            }

            try
            {
                await _next(context);
            }
            finally
            {
                var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                var statusCode = context.Response.StatusCode;

                var orderNumber = context.Items["OrderNumber"]?.ToString();
                var userId = context.Items["UserId"]?.ToString();

                if (statusCode == 200 && !string.IsNullOrEmpty(orderNumber))
                {
                    _logger.LogInformation(
                        "User:{User} | UserId:{UserId} | OrderNumber:{OrderNumber} | {Method} {Url} | Status:{Status} | {Duration}ms",
                        username, userId, orderNumber, method, url, statusCode, duration);
                }
                else
                {
                    _logger.LogInformation(
                        "User:{User} | {Method} {Url} | Status:{Status} | {Duration}ms",
                        username, method, url, statusCode, duration);
                }
            }
        }
    }
}
