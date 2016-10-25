using AlexaSkillsKit.Speechlet.Abstractions;
using ServiceStack.Web;

namespace AlexaSkillsKit.Lib.ServiceStack.Adapters
{
    public class ServiceStackRequestAdapter : IAbstractHttpRequest
    {
        private readonly IRequest _request;

        public ServiceStackRequestAdapter(IRequest request)
        {
            _request = request;
        }

        public IHttpContent Content { get { return new ServiceStackContent(_request);}  }
        public IHttpHeaders Headers { get { return new ServiceStackHeaders(_request);}  }
    }
}