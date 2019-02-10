using System;
using System.IO;
using System.Linq;
using Dtos.Schedule;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using Services.Abstract;

namespace Services
{
    public class ScheduleParserService : IScheduleParserService
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public ScheduleParserService(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public Agenda ParseSchedule()
        {
            var agenda = new Agenda();
            var downloadsPath = $"{hostingEnvironment.ContentRootPath}/../../downloads/";

            var files = Directory.GetFiles(downloadsPath);
            if (files.Any())
            {
                try
                {
                    var scheduleFile = new FileInfo(files.First());
                    using (var excelPackage = new ExcelPackage(scheduleFile))
                    {
                        if (excelPackage.Workbook.Worksheets.Any())
                        {
                            var worksheet = excelPackage.Workbook.Worksheets.Last();

                            int rowCount = worksheet.Dimension.Rows;

                            // Parse the whole schedule
                            for (int rowIndex = 2; rowIndex <= rowCount; rowIndex++)
                            {
                                var nickname = worksheet.Cells[rowIndex, 1].Value;
                                if (nickname == null)
                                {
                                    break;
                                }

                                var scheduledTime = new ScheduledTime();
                                scheduledTime.EngineerNickname = nickname.ToString().Trim();
                                scheduledTime.From = DateTime.FromOADate((double)worksheet.Cells[rowIndex, 2].Value);
                                scheduledTime.To = DateTime.FromOADate((double)worksheet.Cells[rowIndex, 3].Value);

                                //Todo extract time from 4th column
                                var dayOfWeek = worksheet.Cells[rowIndex, 4].Value.ToString();
                                var timeOfDay = dayOfWeek.Replace(scheduledTime.From.DayOfWeek.ToString(), string.Empty).Split('-');

                                var startTime = DateTime.Parse(timeOfDay[0]);
                                var endTime = DateTime.Parse(timeOfDay[1]);
                                scheduledTime.From += startTime.TimeOfDay;
                                scheduledTime.To += endTime.TimeOfDay;

                                scheduledTime.Hours = (double)worksheet.Cells[rowIndex, 5].Value;

                                agenda.Schedule.Add(scheduledTime);
                            }

                            // Parse all engineers
                            for (int rowIndex = 2; rowIndex < rowCount; rowIndex++)
                            {
                                var engineerDetails = worksheet.Cells[rowIndex, 9].Value;
                                if (engineerDetails == null)
                                {
                                    break;
                                }

                                var detailsParts = engineerDetails.ToString().Split('-');
                                var engineer = new Engineer();
                                engineer.Nickname = detailsParts[0].Trim();
                                engineer.Name = detailsParts[1].Trim();
                                engineer.Email = detailsParts[2].Trim();

                                agenda.Engineers.Add(engineer);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return agenda;
        }
    }
}
