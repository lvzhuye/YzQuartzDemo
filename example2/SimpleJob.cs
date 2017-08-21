using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace example2
{
    public class SimpleJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobKey jobkey = context.JobDetail.Key;
            //tosting("r")?????
            Debug.WriteLine("Simple says:{0} executing at {1}",jobkey,DateTime.Now.ToString("r"));
        }
    }
}
