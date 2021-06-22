using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ContractManagementSystem.Controllers
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().GetAwaiter().GetResult();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<ContractExpiry>().Build();
            IJobDetail job2 = JobBuilder.Create<AlertsConfiguration>().Build();
            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithDailyTimeIntervalSchedule
            //      (s =>
            //         s.WithIntervalInHours(12)
            //        .OnEveryDay()
            //        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(2, 8))
            //      )
            //    .Build();
            int Hours = 3;
            int Minutes = 0;


            try
            {

                Hours = Convert.ToInt32(WebConfigurationManager.AppSettings["ScheduleHours"]);
                Minutes = Convert.ToInt32(WebConfigurationManager.AppSettings["ScheduleMinutes"]);
                
            }
            catch { }
            //int ApplicationLink = WebConfigurationManager.AppSettings["ApplicationLink"];


            int Alert_Hours = 2;
            int Alert_Minutes = 0;


            try
            {


                Alert_Hours = Convert.ToInt32(WebConfigurationManager.AppSettings["ScheduleHours_ForAlerts"]);
                Alert_Minutes = Convert.ToInt32(WebConfigurationManager.AppSettings["ScheduleMinutes_ForAlerts"]);
            }
            catch { }

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInHours(Hours)
                .WithIntervalInMinutes(Minutes)
                .RepeatForever())
            .Build();

            scheduler.ScheduleJob(job, trigger);

            ITrigger trigger2 = TriggerBuilder.Create()
            .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(Alert_Hours, Alert_Minutes))
                  )
                .Build();

            scheduler.ScheduleJob(job2, trigger2);


        }
    }
}