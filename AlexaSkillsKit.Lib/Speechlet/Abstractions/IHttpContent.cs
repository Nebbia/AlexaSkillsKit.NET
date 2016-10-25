using System.Threading.Tasks;

namespace AlexaSkillsKit.Speechlet.Abstractions
{
    public interface IHttpContent
    {
        Task<byte[]> ReadAsByteArrayAsync();
    }
}