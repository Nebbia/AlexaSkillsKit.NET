using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using AlexaSkillsKit.Speechlet.Abstractions;
using ServiceStack;
using ServiceStack.Web;

namespace AlexaSkillsKit.Lib.ServiceStack.Adapters
{
    public class ResponseFactory : IHttpResponseFactory<HttpResult> 
    {
        public HttpResult BadRequest(string reason)
        {
            return new HttpResult()
            {
                StatusCode = HttpStatusCode.BadRequest,
                StatusDescription = reason
            };
        }

        public HttpResult InternalServerError()
        {
            return new HttpResult()
            {
                StatusCode = HttpStatusCode.InternalServerError
            };
        }

        public HttpResult Ok(string alexaResponse)
        {
            using (var jsonStream = GenerateStreamFromString(alexaResponse))
            {
                return new HttpResult(jsonStream, "application/json");
            }
        }

        private MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }
    }
}