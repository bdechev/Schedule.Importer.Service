using System;
using System.IO;
using System.Linq;
using Dtos.Schedule;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using Services.Abstract;
using Microsoft.Extensions.Configuration;

namespace Services
{
    public class ScheduleParserService : IScheduleParserService
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;

        public ScheduleParserService(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
        }

        public Agenda ParseSchedule()
        {
            var agenda = new Agenda();
            var mode = configuration.GetSection("RunMode").Value;
            var downloadsPath = mode == "localhost" ? "C:\\FOURTH\\Mine\\OOH" : $"{hostingEnvironment.ContentRootPath}/../../downloads/";

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
                            //Change December to currentMonth
                            var currentMonth = "december"; // DateTime.Now.ToString("MMMM").ToLower();
                            
                            var worksheet = excelPackage.Workbook.Worksheets.Single(x => x.Name.ToLower().Equals(currentMonth));

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
                                scheduledTime.To = scheduledTime.From;

                                //Todo extract time from 4th column
                                var dayOfWeek = worksheet.Cells[rowIndex, 3].Value.ToString();
                                var fromToHours = worksheet.Cells[rowIndex, 4].Value.ToString();
                                var timeOfDay = fromToHours.Split('-');
                                if (timeOfDay.Any(time => time.Contains("24:00")))
                                {
                                    timeOfDay[1] = "00:00";
                                    scheduledTime.To = scheduledTime.From.AddDays(1);
                                }

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
                                    continue;
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
