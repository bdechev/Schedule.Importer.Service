using System;
using System.IO;
using System.Text;
using Dtos.Schedule;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Microsoft.AspNetCore.Hosting;
using Services.Abstract;

namespace Services
{
    public class CalService : ICalService
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public CalService(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async void CreateAppointments(Agenda agenda)
        {
            var calendar = new Calendar();
            foreach (var scheduledTime in agenda.Schedule)
            {
                calendar.Events.Add(new CalendarEvent
                {
                    Uid = Guid.NewGuid().ToString(),
                    Name = "OOH_Duty_Hours",
                    Class = "PUBLIC",
                    Summary = "some summary",
                    Description = "some description",
                    Created = new CalDateTime(DateTime.UtcNow),
                    Start = new CalDateTime(scheduledTime.From),
                    End = new CalDateTime(scheduledTime.To),
                    Sequence = 0,
                    Location = "some location"
                });
            }
            var serializer = new CalendarSerializer(new SerializationContext());
            var serializedCalendar = serializer.SerializeToString(calendar);
            var bytesCalendar = Encoding.UTF8.GetBytes(serializedCalendar);
            var contentStream = new MemoryStream(bytesCalendar);

            var calendarsPath = $"{hostingEnvironment.ContentRootPath}/../../calendars/";
            using (Stream stream = new FileStream($"{calendarsPath}/{calendar.Name}_{DateTime.Now.ToShortDateString()}", FileMode.Create, FileAccess.Write, FileShare.None, (int)contentStream.Length, true))
            {
                await contentStream.CopyToAsync(stream);
            }

            contentStream.Close();
        }
    }
}
