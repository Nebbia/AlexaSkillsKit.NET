using System.Threading.Tasks;
using AlexaSkillsKit.Lib.ServiceStack.Adapters;
using ServiceStack;

namespace AlexaSkillsKit.Sample.ServiceStack.Services
{
    public class SkillService : Service
    {
        public object Post(AlexaSkillRequest request)
        {
            var alexaSkill = new AlexaSkill();
            return alexaSkill.GetResponse(new ServiceStackRequestAdapter(Request));
        }
    }
}