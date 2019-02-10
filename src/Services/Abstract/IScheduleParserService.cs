using Dtos.Schedule;

namespace Services.Abstract
{
    public interface IScheduleParserService
    {
        Agenda ParseSchedule();
    }
}
