using Newtonsoft.Json;
using Quartz;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Interfaces;
using Rubix.Explorer.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.Explorer.API
{
    [DisallowConcurrentExecution]
    public class LevelBasedTokenJob : IJob
    {
        private readonly IRepositoryRubixToken _repositoryRubixToken;
        private readonly ILevelBasedTokenRepository _levelBasedTokenRepository;
        public LevelBasedTokenJob(IRepositoryRubixToken repositoryRubixToken, ILevelBasedTokenRepository levelBasedTokenRepository)
        {
            _repositoryRubixToken = repositoryRubixToken;
            _levelBasedTokenRepository = levelBasedTokenRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("******************LevelBasedTokens Job Start***************************");
            var levelBasedTokensList = _levelBasedTokenRepository.GetAllAsync();
            var rubixTokensList = _repositoryRubixToken.GetAllAsync().Result.Select(x => x.Level);
            var groupedTokensList = (from rbxTokens in rubixTokensList
                                     group rbxTokens by rbxTokens into newTokensGroup
                                     select new
                                     {
                                         Level = newTokensGroup.Key,
                                         count = newTokensGroup.Count()
                                     });
            var Level1 = "Level 1";
            var LevelCount = 0;
            List<LevelBasedTokensDto> levelBasedTokens = new List<LevelBasedTokensDto>();
            foreach (var item in groupedTokensList)
            {
                if (item.Level == "01" || item.Level == "1" || item.Level == "Level1")
                {
                    LevelCount = LevelCount + item.count;
                }
                else
                {
                    levelBasedTokens.Add(new LevelBasedTokensDto()
                    {
                        Level = "Level " + item.Level,
                        Count = item.count
                    });
                }
            }
            var level1Record = new LevelBasedTokensDto() { Level = Level1, Count = LevelCount };
            levelBasedTokens.Add(level1Record);
            var totalLevelBasedData = levelBasedTokens.OrderBy(x => x.Level);
            var levelBasedTokensData = levelBasedTokensList.Result.Where(x => x.CreationTime.Value.Date == DateTime.Today.Date).FirstOrDefault();
            if (levelBasedTokensData != null)
            {
                levelBasedTokensData.Data = JsonConvert.SerializeObject(levelBasedTokens);
                levelBasedTokensData.LastModificationTime = DateTime.UtcNow;
                await _levelBasedTokenRepository.UpdateAsync(levelBasedTokensData);
            }
            else
            {
                await _levelBasedTokenRepository.InsertAsync(new LevelBasedTokens()
                {
                    Data = JsonConvert.SerializeObject(levelBasedTokens),
                    CreationTime = DateTime.UtcNow,
                    LastModificationTime = DateTime.UtcNow
                });
            }

            Console.WriteLine("****************Dashboard Completed*****************");
        }
    }
}
