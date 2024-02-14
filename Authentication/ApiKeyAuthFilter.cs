using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api_with_auth.Authentication
{
    public class ApiKeyAuthFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        public ApiKeyAuthFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstans.ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult(AuthConstans.messageForMissingKey);
                return;
            }

            var apiKey = _configuration.GetValue<string>(AuthConstans.ApiKeySectionName);
            if (!apiKey!.Equals(extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult(AuthConstans.messageForInvalidKey);
                return;
            }
        }
    }
}