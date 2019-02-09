using System.Threading.Tasks;
using Schedule.Importer.Service.Configuration.Abstract;
using Services.Abstract;

namespace Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IAtlassianConfig _atlassianConfig;

        public ScheduleService(IAtlassianConfig atlassianConfig)
        {
            _atlassianConfig = atlassianConfig;
        }

        public async Task UpdateSchedule()
        {
            
        }
    }
}
