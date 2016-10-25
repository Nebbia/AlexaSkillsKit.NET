namespace AlexaSkillsKit.Speechlet.Abstractions
{
    public interface IHttpResponseFactory<out TResponse>
    {
        TResponse BadRequest(string reason);
        TResponse InternalServerError();
        TResponse Ok(string alexaResponse);
    }
}