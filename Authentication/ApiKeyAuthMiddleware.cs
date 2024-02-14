namespace api_with_auth.Authentication
{
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(AuthConstans.ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(AuthConstans.messageForMissingKey);
                return;
            }

            var apiKey = _configuration.GetValue<string>(AuthConstans.ApiKeySectionName);
            if (!apiKey!.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(AuthConstans.messageForInvalidKey);
                return;
            }

            await _next(context);
        }
    }
}