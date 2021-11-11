using Newtonsoft.Json;
using Quartz;
using Rubix.API.Shared.Enums;
using Rubix.API.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.Explorer.API
{
 
    [DisallowConcurrentExecution]
    public class RubixDashboardJob : IJob
    {
        private readonly IRepositoryDashboard _repositoryDashboard;
        private readonly IRepositoryRubixTransaction _repositoryRubixTransaction;

        public RubixDashboardJob(IRepositoryDashboard repositoryDashboard, IRepositoryRubixTransaction repositoryRubixTransaction)
        {
            _repositoryDashboard = repositoryDashboard;
            _repositoryRubixTransaction = repositoryRubixTransaction;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("*********************************************");
            //TODO Transaction , Tokens Live charts data updates.

            var values = EnumUtil.GetValues<ActivityFilter>();
            foreach (var activeity in values)
            {
                Console.WriteLine(activeity);
                var data= await _repositoryRubixTransaction.GetAllByFilterAsync(activeity);
                var serilizedData = JsonConvert.SerializeObject(data);
                Console.WriteLine(serilizedData);
            }
        }
    }
}
