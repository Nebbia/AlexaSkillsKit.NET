using AlexaSkillsKit.Lib.ServiceStack.Adapters;
using ServiceStack;

namespace AlexaSkillsKit.Sample.ServiceStack
{
    public class SkillService : Service
    {
        public object Post(AlexaSkillRequest request)
        {
            var alexaSkill = new AlexaSkill();
            return alexaSkill.GetResponse(new RequestAdapter(Request));
        }
    }
}