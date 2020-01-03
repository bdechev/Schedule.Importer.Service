using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Services.Abstract;

namespace Services
{
    public class CalendarService : ICalendarService
    {
        const string calendarFolder = "calendars";

        public Task<FileInfo> GetEmployeeLatestCalendar(string name)
        {
            var basePath = Directory.GetParent(Environment.CurrentDirectory).Parent?.FullName;
            if (basePath == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var calendarsPath = Path.Combine(basePath, calendarFolder);
            var availableCalendars = new DirectoryInfo(calendarsPath).GetFiles();

            var availableEmployeesNames = ExtractEmployeeNameFromFileName(availableCalendars);

            FileInfo fileToReturn = null;
            if (availableCalendars.Any() && availableEmployeesNames.Any(a => a.ToLower().Equals(name.ToLower())))
            {
                var latestDateFromFileName = GetLatestDateFromFileName(availableCalendars.Select(x => x.FullName));
                fileToReturn = availableCalendars.First(x => x.FullName.Contains(latestDateFromFileName));
            }

            return Task.FromResult(fileToReturn);
        }

        public IEnumerable<string> GetAvailableCalendars()
        {
            var directoryInfo = Directory.GetParent(Environment.CurrentDirectory).Parent;
            var availableCalendars = new FileInfo[] { };
            if (directoryInfo != null)
            {
                var basePath = directoryInfo.FullName;
                var calendarsPath = Path.Combine(basePath, calendarFolder);
                availableCalendars = new DirectoryInfo(calendarsPath).GetFiles();
            }

            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return availableCalendars.Select(x => x.Name);
        }

        private string GetLatestDateFromFileName(IEnumerable<string> fileNamesList)
        {
            var numbers = "0123456789".ToCharArray();
            var datesList = new List<DateTime>();
            foreach (var fileName in fileNamesList)
            {
                var indexFirstNumber = fileName.IndexOfAny(numbers);
                var indexLastNumber = fileName.LastIndexOfAny(numbers);
                var extractedDateFromFilename = fileName.Remove(indexLastNumber + 1).Substring(indexFirstNumber);
                DateTime.TryParse(extractedDateFromFilename, out var date);
                datesList.Add(date);
            }

            return datesList.OrderByDescending(x => x).First().ToString("yyyy-MM-dd");
        }

        private IEnumerable<string> ExtractEmployeeNameFromFileName(IEnumerable<FileInfo> files)
        {
            return files.Select(x => x.Name)
                .Select(n => n.Remove(n.IndexOf('_'))).Distinct()
                .Select(r => r.Replace(" ", string.Empty));
        }
    }
}