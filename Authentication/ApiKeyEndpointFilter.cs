using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

namespace api_with_auth.Authentication
{
    public class ApiKeyEndpointFilter : IEndpointFilter
    {
        private readonly IConfiguration _configuration;
        public ApiKeyEndpointFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstans.ApiKeyHeaderName, out var extractedApiKey))
            {
                return new UnauthorizedHttpObjectResult(AuthConstans.messageForMissingKey);
            }

            var apiKey = _configuration.GetValue<string>(AuthConstans.ApiKeySectionName);
            if (!apiKey!.Equals(extractedApiKey))
            {
                return new UnauthorizedHttpObjectResult(AuthConstans.messageForInvalidKey);
            }

            return await next(context);
        }
    }
}