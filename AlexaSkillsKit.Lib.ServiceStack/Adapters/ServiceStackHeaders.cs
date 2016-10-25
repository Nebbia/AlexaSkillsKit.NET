using System.Collections.Generic;
using System.Linq;
using AlexaSkillsKit.Speechlet.Abstractions;
using ServiceStack.Web;

namespace AlexaSkillsKit.Lib.ServiceStack.Adapters
{
    public class ServiceStackHeaders : IHttpHeaders
    {
        private readonly IRequest _request;

        public ServiceStackHeaders(IRequest request)
        {
            _request = request;
        }
        public bool Contains(string header)
        {
            return _request.Headers.AllKeys.Any(x => x == header);
        }

        public IEnumerable<string> GetValue(string header)
        {
            return _request.Headers.GetValues(header);
        }
    }
}