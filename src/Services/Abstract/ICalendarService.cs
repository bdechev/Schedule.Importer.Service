using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Services.Abstract
{
    public interface ICalendarService
    {
        Task<FileInfo> GetEmployeeLatestCalendarByNickname(string nickname);

        IEnumerable<string> GetAvailableCalendars();
    }
}
