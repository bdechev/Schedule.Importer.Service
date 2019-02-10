using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Schedule
{
    public class ScheduledTime
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string EngineerNickname { get; set; }

        public double Hours { get; set; }
    }
}
