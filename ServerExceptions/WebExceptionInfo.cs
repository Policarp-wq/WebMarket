using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace WebMarket.ServerExceptions
{
    public class WebExceptionInfo
    {
        public readonly int StatusCode;
        public readonly string Message;
        public readonly string Title;
        public readonly Dictionary<string, string>? Extensions;

        public WebExceptionInfo(Exception ex, HttpContext context, bool detailed = false)
        {
            StatusCode = context.Response.StatusCode;
            Message = ex.Message;
            Title = ReasonPhrases.GetReasonPhrase(StatusCode);
            if (string.IsNullOrEmpty(Title))
                Title = "Unhandeled status code value";
            if (detailed)
            {
                Extensions = [];
                if (ex.InnerException != null)
                    Extensions["innerException"] = ex.InnerException.Message;
                Extensions["request"] = context.Request.Path;
                Extensions["method"] = context.Request.Method;
            }
        }

        public string ToJson()
        {
            try
            {
                return JsonConvert.SerializeObject(this);
            }
            catch (Exception ex)
            {
                return "Exception raised during serialization: " + ex.Message;
            }

        }
    }
}
