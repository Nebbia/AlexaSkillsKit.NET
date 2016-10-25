using System.Net;
using System.Net.Http;
using System.Text;
using AlexaSkillsKit.Speechlet.Abstractions;

namespace AlexaSkillsKit.Speechlet.Impl
{
    public class ResponseFactory : IHttpResponseFactory<HttpResponseMessage>
    {
        public HttpResponseMessage BadRequest(string reason)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                ReasonPhrase = reason
            };
        }

        public HttpResponseMessage InternalServerError()
        {
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }

        public HttpResponseMessage Ok(string alexaResponse)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(alexaResponse, Encoding.UTF8, "application/json")
            };
            //Debug.WriteLine(httpResponse.ToLogString()); //todo
        }
    }
}