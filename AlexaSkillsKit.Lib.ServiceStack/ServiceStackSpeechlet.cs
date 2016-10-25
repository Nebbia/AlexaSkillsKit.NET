using System;
using AlexaSkillsKit.Authentication;
using AlexaSkillsKit.Json;
using AlexaSkillsKit.Lib.ServiceStack.Adapters;
using AlexaSkillsKit.Speechlet;
using AlexaSkillsKit.Speechlet.Abstractions;
using ServiceStack;
using ServiceStack.Web;

namespace AlexaSkillsKit.Lib.ServiceStack
{
    public abstract class ServiceStackSpeechlet : BaseSpeechlet<RequestAdapter, HttpResult, ResponseFactory, IRequest>
    {
       
    }
}