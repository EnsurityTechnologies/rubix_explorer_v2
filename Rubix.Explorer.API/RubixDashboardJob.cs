using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.Explorer.API
{
 
    [DisallowConcurrentExecution]
    public class RubixDashboardJob : IJob
    {
        public RubixDashboardJob()
        {

        }

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("job executed");
            //TODO Transaction , Tokens Live charts data updates.
            return Task.CompletedTask;
        }
    }
}
