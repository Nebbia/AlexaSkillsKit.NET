using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using AlexaSkillsKit.Speechlet.Abstractions;
using ServiceStack;
using ServiceStack.Web;

namespace AlexaSkillsKit.Lib.ServiceStack.Adapters
{
    public class ServiceStackResponseFactory : IHttpResponseFactory<HttpResult> 
    {
        public IRequest Request { get; set; }

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

        public class ExcelFileResult : IHasOptions, IStreamWriter
        {
            private readonly Stream _responseStream;
            public IDictionary<string, string> Options { get; private set; }

            public ExcelFileResult(Stream responseStream)
            {
                _responseStream = responseStream;

                Options = new Dictionary<string, string> {
             {"Content-Type", "application/json"}
            };
            }

            public void WriteTo(Stream responseStream)
            {
                if (_responseStream == null)
                    return;

                _responseStream.WriteTo(responseStream);
                responseStream.Flush();
            }
        }
    }
}