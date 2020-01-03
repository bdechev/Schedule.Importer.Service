﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Services.Abstract
{
    public interface ICalendarService
    {
        Task<FileInfo> GetEmployeeLatestCalendar(string nickname);

        IEnumerable<string> GetAvailableCalendars();
    }
}
