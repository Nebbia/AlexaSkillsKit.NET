using AlexaSkillsKit.Speechlet.Abstractions;
using ServiceStack.Web;

namespace AlexaSkillsKit.Lib.ServiceStack.Adapters
{
    public class RequestAdapter : IAbstractHttpRequest
    {
        private readonly IRequest _request;

        public RequestAdapter(IRequest request)
        {
            _request = request;
        }

        public IHttpContent Content { get { return new ContentAdapter(_request);}  }
        public IHttpHeaders Headers { get { return new HeadersAdapter(_request);}  }
    }
}