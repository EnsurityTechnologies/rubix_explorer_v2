using Newtonsoft.Json;
using Quartz;
using Rubix.API.Shared;
using Rubix.API.Shared.Common;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Enums;
using Rubix.API.Shared.Interfaces;
using Rubix.API.Shared.Repositories;
using Rubix.Explorer.API.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.Explorer.API
{
 
    [DisallowConcurrentExecution]
    public class RubixDashboardJob : IJob
    {
        private readonly IRepositoryDashboard _repositoryDashboard;

        private readonly IRepositoryCardsDashboard _repositoryCardsDashboard;

        private readonly IRepositoryRubixTransaction _repositoryRubixTransaction;
        private readonly IRepositoryRubixToken _repositoryRubixToken;
        private readonly IRepositoryRubixUser _repositoryRubixUser;


        public RubixDashboardJob(IRepositoryRubixUser repositoryRubixUser, IRepositoryCardsDashboard repositoryCardsDashboard,IRepositoryDashboard repositoryDashboard, IRepositoryRubixTransaction repositoryRubixTransaction, IRepositoryRubixToken repositoryRubixToken)
        {
            _repositoryDashboard = repositoryDashboard;
            _repositoryRubixTransaction = repositoryRubixTransaction;
            _repositoryRubixToken = repositoryRubixToken;
            _repositoryCardsDashboard = repositoryCardsDashboard;
            _repositoryRubixUser = repositoryRubixUser;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("******************* Dashboard Start**************************");

            //Transaction , Tokens Live charts data updates.

            var values = EnumUtil.GetValues<ActivityFilter>();
            foreach (var activeity in values)
            {
                switch (activeity)
                {
                    case ActivityFilter.Today:
                        {

                            Console.WriteLine("dash today completed");
                        }
                        break;
                    case ActivityFilter.Week:
                        {
                            Console.WriteLine("dash Weekly completed");
                            break;
                        }
                    case ActivityFilter.Month:
                        {

                            Console.WriteLine("dash Monthly completed");
                            break;
                        }
                    case ActivityFilter.Year:
                        {


                            Console.WriteLine("dash Quarter completed");
                            break;
                        }
                    case ActivityFilter.All:
                        {
                            Console.WriteLine("dash All completed");
                            break;
                        }
                }
            }
            Console.WriteLine("****************Dashboard Completed*****************");
        }
    }
}
