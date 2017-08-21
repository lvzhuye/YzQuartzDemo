using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace example1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("------Initializing---------");

            ISchedulerFactory sf = new StdSchedulerFactory();

            IScheduler sched = sf.GetScheduler();

            Console.WriteLine("------Initialization Complete----------");

            DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow.ToLocalTime());

            Console.WriteLine("------Scheduling Job-------------");

            IJobDetail job = JobBuilder.Create<HelloJob>()
                            .WithIdentity("job1", "group1")
                            .Build();

            ITrigger trigger = TriggerBuilder.Create()
                             .WithIdentity("trigger1", "group1")
                             .StartAt(runTime)
                             .Build();

            sched.ScheduleJob(job,trigger);

            Console.WriteLine("--------Job Key:"+job.Key.ToString()+"------------");

            sched.Start();

            Console.WriteLine("----------Started Scheduler-----------");

            Thread.Sleep(65*1000);

            Console.WriteLine("-----Shutting Down ------------");

            sched.Shutdown();

            Console.WriteLine("-----Shutdown Complete ----------");

            Console.ReadKey();

        }
    }
}
