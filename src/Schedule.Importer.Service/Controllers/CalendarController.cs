using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;

namespace Schedule.Importer.Service.Controllers
{
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [HttpGet]
        [Route("/GetCalendarByNickname/{nickname}")]
        public async Task<FileResult> GetLatestCalendarForEmployeeByNickname(string nickname)
        {
            var fileToReturn = await _calendarService.GetEmployeeLatestCalendarByNickname(nickname);
            var stream = System.IO.File.OpenRead(fileToReturn.FullName);
            return File(stream, "application/octet-stream", fileToReturn.Name);
        }

        [HttpGet]
        [Route("/ViewCalendars")]
        public async Task<IEnumerable<string>> ViewAvailableCalendars()
        {
            return _calendarService.GetAvailableCalendars();
        }
    }
}