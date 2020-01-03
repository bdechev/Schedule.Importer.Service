using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("/GetCalendar/{nickname}")]
        public async Task<FileResult> GetLatestCalendarForEmployee(string nickname)
        {
            var fileToReturn = await _calendarService.GetEmployeeLatestCalendar(nickname);
            if (fileToReturn == null)
            {
//                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
//                {
//                    Content = new StringContent($"No Employee found with ID = {nickname}")
//                };

                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var stream = System.IO.File.OpenRead(fileToReturn.FullName);
            return File(stream, "application/octet-stream", fileToReturn.Name);
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("/ViewCalendars")]
        public async Task<IEnumerable<string>> ViewAvailableCalendars()
        {
            return _calendarService.GetAvailableCalendars();
        }
    }
}