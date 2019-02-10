using System.Collections.Generic;

namespace Dtos.Schedule
{
    public class Agenda
    {
        public Agenda()
        {
            Schedule = new List<ScheduledTime>();
            Engineers = new List<Engineer>();
        }

        public List<ScheduledTime> Schedule { get; set; }

        public List<Engineer> Engineers { get; set; }
    }
}
