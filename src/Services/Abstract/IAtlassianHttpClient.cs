using System.Threading.Tasks;

namespace Services.Abstract
{
    public interface IAtlassianHttpClient
    {
        Task GetAttachments(string pageId);
    }
}
