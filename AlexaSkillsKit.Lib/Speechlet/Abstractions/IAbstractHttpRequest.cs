namespace AlexaSkillsKit.Speechlet.Abstractions
{
    public interface IAbstractHttpRequest
    {
        IHttpContent Content { get; }
        IHttpHeaders Headers { get; }
    }
}