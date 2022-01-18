using Newtonsoft.Json;
using Quartz;
using Rubix.API.Shared.Dto;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Enums;
using Rubix.API.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.Explorer.API
{
    [DisallowConcurrentExecution]
    public class RubixCardDashboardJob : IJob
    {
        private readonly IRepositoryDashboard _repositoryDashboard;

        private readonly IRepositoryCardsDashboard _repositoryCardsDashboard;

        private readonly IRepositoryRubixTransaction _repositoryRubixTransaction;
        private readonly IRepositoryRubixToken _repositoryRubixToken;
        private readonly IRepositoryRubixUser _repositoryRubixUser;

        public RubixCardDashboardJob(IRepositoryRubixUser repositoryRubixUser, IRepositoryCardsDashboard repositoryCardsDashboard, IRepositoryDashboard repositoryDashboard, IRepositoryRubixTransaction repositoryRubixTransaction, IRepositoryRubixToken repositoryRubixToken)
        {
            _repositoryDashboard = repositoryDashboard;
            _repositoryRubixTransaction = repositoryRubixTransaction;
            _repositoryRubixToken = repositoryRubixToken;
            _repositoryCardsDashboard = repositoryCardsDashboard;
            _repositoryRubixUser = repositoryRubixUser;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("******************Cards Job Start***************************");

            var rubixusers = await _repositoryRubixUser.GetCountAsync();

            var circulatingSupply = await _repositoryRubixToken.GetCountAsync();

            var rubixTokens = await _repositoryRubixToken.GetCountAsync();

            var rubixTrasactions = await _repositoryRubixTransaction.GetCountAsync();

            var cards = await _repositoryCardsDashboard.FindByAsync();
            if (cards != null)
            {
                var obj = new CardsDto()
                {
                    TokensCount = rubixTokens,
                    TransCount = rubixTrasactions,
                    UsersCount = rubixusers,
                    CirculatingSupply = circulatingSupply
                };
                cards.Data = JsonConvert.SerializeObject(obj);
                cards.LastModificationTime = DateTime.UtcNow;
                await _repositoryCardsDashboard.UpdateAsync(cards);
            }
            else
            {
                var obj = new CardsDto()
                {
                    TokensCount = rubixTokens,
                    TransCount = rubixTrasactions + rubixTokens,
                    UsersCount = rubixusers,
                    CirculatingSupply = circulatingSupply
                };

                await _repositoryCardsDashboard.InsertAsync(new CardsDashboard()
                {
                    EntityType = EntityType.Transactions,
                    CreationTime = DateTime.UtcNow,
                    Data = JsonConvert.SerializeObject(obj),
                    LastModificationTime = DateTime.UtcNow
                });
            }
        }
    }
}
