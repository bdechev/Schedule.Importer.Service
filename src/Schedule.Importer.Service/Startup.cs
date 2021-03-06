﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Schedule.Importer.Service.Configuration.Abstract;
using Services;
using Services.Abstract;

namespace Schedule.Importer.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<IAtlassianHttpClient, AtlassianHttpClient>();
            services.AddScoped<IScheduleParserService, ScheduleParserService>();
            services.AddScoped<ICalService, CalService>();
            services.AddSingleton<IAtlassianConfig>(Configuration.GetSection("Atlassian").Get<AtlassianConfig>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
