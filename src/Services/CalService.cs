using System;
using System.IO;
using System.Linq;
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
            var calendar = new Calendar()
            {
                Name = "VCALENDAR",
                ProductId = "//Fourth//ScheduleImporter ical.net 4.0//EN",
                Scale = "GREGORIAN"
            };

            foreach (var scheduledTime in agenda.Schedule)
            {
                var engineer = agenda.Engineers.FirstOrDefault(x => x.Nickname == scheduledTime.EngineerNickname);

                calendar.Events.Add(new CalendarEvent
                {
                    Uid = Guid.NewGuid().ToString(),
                    Name = "VEVENT",
                    Class = "PUBLIC",
                    Summary = $"{engineer.Name} - OOH Appointment",
                    Description = "API & Services - WFM / Our of office hours support appointment",
                    Created = new CalDateTime(DateTime.UtcNow),
                    Start = new CalDateTime(scheduledTime.From),
                    End = new CalDateTime(scheduledTime.To),
                    Sequence = 0,
                    Status = "CONFIRMED",
                });
            }

            var serializer = new CalendarSerializer(new SerializationContext());
            var serializedCalendar = serializer.SerializeToString(calendar);
            var bytesCalendar = Encoding.UTF8.GetBytes(serializedCalendar);
            var contentStream = new MemoryStream(bytesCalendar);

            var calendarsPath = $"{hostingEnvironment.ContentRootPath}/../../calendars/";
            using (Stream stream = new FileStream($"{calendarsPath}/Full_OOH_Schedule_{DateTime.Now.ToString("yyyy-MM-dd").Replace("/", string.Empty)}.ics", FileMode.Create, FileAccess.Write, FileShare.None, (int)contentStream.Length, true))
            {
                await contentStream.CopyToAsync(stream);
            }

            contentStream.Close();

            foreach (var engineer in agenda.Engineers)
            {
                var engineerCalendar = new Calendar()
                {
                    Name = "VCALENDAR",
                    ProductId = "//Fourth//ScheduleImporter ical.net 4.0//EN",
                    Scale = "GREGORIAN"
                };

                foreach (var scheduledTime in agenda.Schedule.Where(x => x.EngineerNickname == engineer.Nickname))
                {
                    engineerCalendar.Events.Add(new CalendarEvent
                    {
                        Uid = Guid.NewGuid().ToString(),
                        Name = "VEVENT",
                        Class = "PUBLIC",
                        Summary = $"{engineer.Name} - OOH Appointment",
                        Description = "API & Services - WFM / Our of office hours support appointment",
                        Created = new CalDateTime(DateTime.UtcNow),
                        Start = new CalDateTime(scheduledTime.From),
                        End = new CalDateTime(scheduledTime.To),
                        Sequence = 0,
                        Status = "CONFIRMED",
                    });
                }

                var engineerSerializer = new CalendarSerializer(new SerializationContext());
                var engineerSerializedCalendar = engineerSerializer.SerializeToString(engineerCalendar);
                var engineerBytesCalendar = Encoding.UTF8.GetBytes(engineerSerializedCalendar);
                var engineerContentStream = new MemoryStream(engineerBytesCalendar);

                var engineerCalendarsPath = $"{hostingEnvironment.ContentRootPath}/../../calendars/";
                using (Stream stream = new FileStream($"{engineerCalendarsPath}/{engineer.Name}_OOH_Schedule__{DateTime.Now.ToString("yyyy-MM-dd").Replace("/", string.Empty)}.ics", FileMode.Create, FileAccess.Write, FileShare.None, (int)engineerContentStream.Length, true))
                {
                    await engineerContentStream.CopyToAsync(stream);
                }

                engineerContentStream.Close();
            }
        }
    }
}