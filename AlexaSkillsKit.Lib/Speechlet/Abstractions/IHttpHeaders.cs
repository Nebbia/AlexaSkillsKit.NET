using System.Collections.Generic;

namespace AlexaSkillsKit.Speechlet.Abstractions
{
    public interface IHttpHeaders
    {
        bool Contains(string header);
        IEnumerable<string> GetValue(string header);
    }
}