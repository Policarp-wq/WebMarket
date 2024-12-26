using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebMarket.Authorization
{
    public class ApiAuthFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _cofniguration;

        public ApiAuthFilter(IConfiguration cofniguration)
        {
            _cofniguration = cofniguration;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            return;
            if (!context.HttpContext.Response.Headers.TryGetValue(AuthConstants.ApiHeader, out var extractedApiKey)) 
            {
                context.Result = new UnauthorizedObjectResult("No api key header!");
                return;
            }
            string? apiKey = _cofniguration.GetValue<string>(AuthConstants.ApiKeySectionName);
            if(apiKey == null) 
                return;
            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("Wrong api key");
                return;
            }

            
        }
    }
}
