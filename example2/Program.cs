using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example2
{
    class Program
    {
        static void Main(string[] args)
        {

            ISchedulerFactory sf = new StdSchedulerFactory();

            IScheduler sch = sf.GetScheduler();

            IJobDetail job = JobBuilder.Create<SimpleJob>()
                            .WithIdentity("job1", "group1")
                            .Build();

            DateTimeOffset timeDate = DateBuilder.EvenMinuteDate(DateTime.Now);

            ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                            .WithIdentity("trigger1", "group1")
                            .StartAt(timeDate)
                            .Build();

            DateTimeOffset? ft = sch.ScheduleJob(job, trigger);

            Console.WriteLine(job.Key+
                "will run at: "+ft+
                "and repeat: " +trigger.RepeatCount +
                "times,every"+trigger.RepeatInterval.TotalSeconds+" seconds");

            job = JobBuilder.Create<SimpleJob>()
                .WithIdentity("job2", "group1")
                .Build();

            DateTimeOffset startTime = DateBuilder.NextGivenSecondDate(null, 15);

            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger2", "group1")
                .StartAt(startTime)
                .Build();

            ft = sch.ScheduleJob(job, trigger);

            Console.WriteLine(job.Key +
                " will run at： " + ft +
                " and repeat " + trigger.RepeatCount +
                " times,every " + trigger.RepeatInterval.TotalSeconds + " seconds");

            job = JobBuilder.Create<SimpleJob>()
                            .WithIdentity("job3", "group1")
                            .Build();

            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                    .WithIdentity("trigger3", "group1")
                    .StartAt(startTime)
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).WithRepeatCount(10))
                    .Build();

            ft = sch.ScheduleJob(job, trigger);

            Console.WriteLine(job.Key +
                " will run at： " + ft +
                " and repeat " + trigger.RepeatCount +
                " times,every " + trigger.RepeatInterval.TotalSeconds + " seconds");


            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                    .WithIdentity("trigger3", "group2")
                    .StartAt(startTime)
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).WithRepeatCount(2))
                    .ForJob(job)
                    .Build();

            ft = sch.ScheduleJob(trigger);

            Console.WriteLine(job.Key +
                " will run at： " + ft +
                " and repeat " + trigger.RepeatCount +
                " times,every " + trigger.RepeatInterval.TotalSeconds + " seconds");

            job = JobBuilder.Create<SimpleJob>()
                        .WithIdentity("job4", "group1")
                        .Build();

            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                        .WithIdentity("trigger4", "group1")
                        .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).WithRepeatCount(5))
                        .Build();

            ft = sch.ScheduleJob(job, trigger);

            Console.WriteLine(job.Key +
                " will run at： " + ft +
                " and repeat " + trigger.RepeatCount +
                " times,every " + trigger.RepeatInterval.TotalSeconds + " seconds");

            job = JobBuilder.Create<SimpleJob>()
                .WithIdentity("job5", "group1")
                .Build();

            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                      .WithIdentity("trigger5", "group1")
                      .StartAt(DateBuilder.FutureDate(5, IntervalUnit.Minute))
                      .Build();

            ft = sch.ScheduleJob(job, trigger);

            Console.WriteLine(job.Key +
                " will run at： " + ft +
                " and repeat " + trigger.RepeatCount +
                " times,every " + trigger.RepeatInterval.TotalSeconds + " seconds");

            job = JobBuilder.Create<SimpleJob>()
                .WithIdentity("job6", "group1")
                .Build();

            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger6", "group1")
                .StartAt(startTime)
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(20).RepeatForever())
                .Build();

            ft = sch.ScheduleJob(job,trigger);

            Console.WriteLine(job.Key +
                " will run at： " + ft +
                " and repeat " + trigger.RepeatCount +
                " times,every " + trigger.RepeatInterval.TotalSeconds + " seconds");

            Console.WriteLine("----------Scheduler Start-----------");

            sch.Start();

            job = JobBuilder.Create<SimpleJob>()
                .WithIdentity("job7", "group1")
                .Build();

            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger7", "group1")
                .StartAt(startTime)
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(5).WithRepeatCount(20))
                .Build();

            ft = sch.ScheduleJob(job, trigger);

            Console.WriteLine(job.Key +
                " will run at： " + ft +
                " and repeat " + trigger.RepeatCount +
                " times,every " + trigger.RepeatInterval.TotalSeconds + " seconds");

            job = JobBuilder.Create<SimpleJob>()
                .WithIdentity("job8", "group1")
                .StoreDurably()
                .Build();

            sch.AddJob(job, true);

            sch.TriggerJob(new JobKey("job8", "group1"));

            Thread.Sleep(500000);

            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger7", "group1")
                .StartAt(startTime)
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).WithRepeatCount(20))
                .Build();

            Console.WriteLine(DateTime.Now);

            ft = sch.RescheduleJob(trigger.Key,trigger);

            Console.WriteLine("job7 rescheduled to run at "+ft);

            SchedulerMetaData schMd = sch.GetMetaData();

            Console.WriteLine(schMd.NumberOfJobsExecuted);

            Thread.Sleep(100000);

            schMd = sch.GetMetaData();

            Console.WriteLine(schMd.NumberOfJobsExecuted);

            Console.ReadKey();

            sch.Shutdown();
        }
    }
}
