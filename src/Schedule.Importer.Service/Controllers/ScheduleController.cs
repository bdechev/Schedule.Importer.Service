﻿using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Schedule.Importer.Service.Controllers
{
    [ApiController]
    public class ScheduleController : Controller
    {
        private readonly IScheduleService scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            this.scheduleService = scheduleService;
        }

        [HttpGet("/schedule")]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        [HttpPost("/schedule/{id}/update")]
        public async Task<IActionResult> Update(string id)
        {
            await scheduleService.UpdateSchedule();

            return Ok();
        }
    }
}
