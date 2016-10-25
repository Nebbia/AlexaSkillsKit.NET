using System;
using AlexaSkillsKit.Authentication;
using AlexaSkillsKit.Json;
using AlexaSkillsKit.Lib.ServiceStack;
using AlexaSkillsKit.Speechlet;
using AlexaSkillsKit.UI;

namespace AlexaSkillsKit.Sample.ServiceStack
{
    public class AlexaSkill : ServiceStackSpeechlet
    {
        public override SpeechletResponse OnIntent(IntentRequest intentRequest, Session session)
        {
            return new SpeechletResponse()
            {
                OutputSpeech = new PlainTextOutputSpeech()
                {
                    Text = "Alexa skill using service stack works!!"
                },
                ShouldEndSession = true
            };
        }

        public override SpeechletResponse OnLaunch(LaunchRequest launchRequest, Session session)
        {
            return new SpeechletResponse();
        }

        public override void OnSessionStarted(SessionStartedRequest sessionStartedRequest, Session session)
        {
            
        }

        public override void OnSessionEnded(SessionEndedRequest sessionEndedRequest, Session session)
        {
            
        }

#if Local
        public override bool OnRequestValidation(SpeechletRequestValidationResult result, DateTime referenceTimeUtc,
            SpeechletRequestEnvelope requestEnvelope)
        {
            return true;
        }
#endif
    }
}