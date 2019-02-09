using System.Threading.Tasks;
using Schedule.Importer.Service.Configuration.Abstract;
using Services.Abstract;

namespace Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IAtlassianHttpClient _atlassianHttpClient;

        public ScheduleService(IAtlassianHttpClient atlassianHttpClient)
        {
            _atlassianHttpClient = atlassianHttpClient;
        }

        public async Task UpdateSchedule()
        {
            await _atlassianHttpClient.GetAttachments();
        }
    }
}
