using Services.Abstract;
using System.Threading.Tasks;

namespace Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IAtlassianHttpClient atlassianHttpClient;
        private readonly IScheduleParserService scheduleParserService;

        public ScheduleService(IAtlassianHttpClient atlassianHttpClient, IScheduleParserService scheduleParserService)
        {
            this.atlassianHttpClient = atlassianHttpClient;
            this.scheduleParserService = scheduleParserService;
        }

        public async Task UpdateSchedule()
        {
            var pageId = "1148387756";
            await atlassianHttpClient.GetAttachments(pageId);

            var agenda = scheduleParserService.ParseSchedule();
        }
    }
}
