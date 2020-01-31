using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Services.Abstract;

namespace Services
{
    public class CalendarService : ICalendarService
    {
        private static readonly string BasePath = Directory.GetParent(Environment.CurrentDirectory).Parent?.FullName;

        public async Task<FileInfo> GetEmployeeLatestCalendarByNickname(string nickname)
        {
            var availableCalendars = new DirectoryInfo(GetCalendarsPath()).GetFiles().Where(f => f.Name.ToLower().Contains(nickname.ToLower()));
            var maxDate = new DateTime(2000, 01, 01);
            FileInfo latestCalendar = null;
            foreach (var availableCalendar in availableCalendars)
            {
                var currentCalendarDate = ExtractDateFromCalendarFileName(availableCalendar.Name);
                if (currentCalendarDate > maxDate)
                {
                    maxDate = currentCalendarDate;
                    latestCalendar = new FileInfo(availableCalendar.FullName);
                }
            }

            return latestCalendar;
        }

        public IEnumerable<string> GetAvailableCalendars()
        {
            var availableCalendars = new DirectoryInfo(GetCalendarsPath()).GetFiles();
            return availableCalendars.Select(x => x.Name);
        }

        private static string GetCalendarsPath()
        {
            var path = Path.Combine(BasePath, "calendars");
            return path;
        }

        private static DateTime ExtractDateFromCalendarFileName(string calendarFilename)
        {
            return DateTime.Parse(calendarFilename.Remove(calendarFilename.IndexOf('.')).Substring(calendarFilename.IndexOf('_') + 1));
        }
    }
}