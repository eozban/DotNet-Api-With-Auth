using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api_with_auth.Authentication
{
    public class ApiKeyAuthAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstans.ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult(AuthConstans.messageForMissingKey);
                return;
            }

            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = configuration.GetValue<string>(AuthConstans.ApiKeySectionName);
            if (!apiKey!.Equals(extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult(AuthConstans.messageForInvalidKey);
                return;
            }
        }
    }
}