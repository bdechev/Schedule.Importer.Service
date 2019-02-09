using System.Threading.Tasks;

namespace Services.Abstract
{
    public interface IAtlassianHttpClient
    {
        Task GetAttachments();
    }
}
