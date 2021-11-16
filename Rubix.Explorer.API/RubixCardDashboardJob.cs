using Newtonsoft.Json;
using Quartz;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Enums;
using Rubix.API.Shared.Interfaces;
using Rubix.Explorer.API.Dtos;
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
            Console.WriteLine("******************Cards Job***************************");

            var rubixusers = await _repositoryRubixUser.GetCountAsync();

            var circulatingSupply = await _repositoryRubixTransaction.GetCountAsync();

            var values = EnumUtil.GetValues<ActivityFilter>();
            foreach (var activeity in values)
            {
                switch (activeity)
                {
                    case ActivityFilter.Today:
                        {
                            var rubixTokens = await _repositoryRubixToken.GetCountByFilterAsync(activeity);

                            var rubixTrasactions = await _repositoryRubixTransaction.GetCountByFilterAsync(activeity);

                            var cards = await _repositoryCardsDashboard.FindByAsync(ActivityFilter.Today);
                            if (cards != null)
                            {
                                var obj = new CardsDto()
                                {
                                    TokensCount = rubixTokens,
                                    TransCount = rubixTrasactions,
                                    UsersCount = rubixusers,
                                    CirculatingSupply= circulatingSupply
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
                                    TransCount = rubixTrasactions,
                                    UsersCount = rubixusers,
                                    CirculatingSupply = circulatingSupply
                                };

                                await _repositoryCardsDashboard.InsertAsync(new CardsDashboard()
                                {
                                    ActivityFilter = ActivityFilter.Today,
                                    EntityType = EntityType.Transactions,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(obj),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }
                        }
                        Console.WriteLine("today completed");
                        break;
                    case ActivityFilter.Weekly:
                        {
                            var rubixTokens = await _repositoryRubixToken.GetCountByFilterAsync(activeity);

                            var rubixTrasactions = await _repositoryRubixTransaction.GetCountByFilterAsync(activeity);

                            var cards = await _repositoryCardsDashboard.FindByAsync(ActivityFilter.Weekly);
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
                                    TransCount = rubixTrasactions,
                                    UsersCount = rubixusers,
                                    CirculatingSupply = circulatingSupply
                                };

                                await _repositoryCardsDashboard.InsertAsync(new CardsDashboard()
                                {
                                    ActivityFilter = ActivityFilter.Weekly,
                                    EntityType = EntityType.Transactions,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(obj),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }
                        }
                        Console.WriteLine("Weekly completed");
                        break;
                    case ActivityFilter.Monthly:
                        {
                            var rubixTokens = await _repositoryRubixToken.GetCountByFilterAsync(activeity);

                            var rubixTrasactions = await _repositoryRubixTransaction.GetCountByFilterAsync(activeity);

                            var cards = await _repositoryCardsDashboard.FindByAsync(ActivityFilter.Monthly);
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
                                    TransCount = rubixTrasactions,
                                    UsersCount = rubixusers,
                                    CirculatingSupply = circulatingSupply
                                };

                                await _repositoryCardsDashboard.InsertAsync(new CardsDashboard()
                                {
                                    ActivityFilter = ActivityFilter.Monthly,
                                    EntityType = EntityType.Transactions,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(obj),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }

                        }

                        Console.WriteLine("Monthly completed");
                        break;
                    case ActivityFilter.Quarterly:
                        {
                            var rubixTokens = await _repositoryRubixToken.GetCountByFilterAsync(activeity);

                            var rubixTrasactions = await _repositoryRubixTransaction.GetCountByFilterAsync(activeity);

                            var cards = await _repositoryCardsDashboard.FindByAsync(ActivityFilter.Quarterly);
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
                                    TransCount = rubixTrasactions,
                                    UsersCount = rubixusers,
                                    CirculatingSupply = circulatingSupply
                                };

                                await _repositoryCardsDashboard.InsertAsync(new CardsDashboard()
                                {
                                    ActivityFilter = ActivityFilter.Quarterly,
                                    EntityType = EntityType.Transactions,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(obj),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }
                        }
                        Console.WriteLine("Quarter completed");
                        break;
                    case ActivityFilter.HalfYearly:
                        {
                            var rubixTokens = await _repositoryRubixToken.GetCountByFilterAsync(activeity);

                            var rubixTrasactions = await _repositoryRubixTransaction.GetCountByFilterAsync(activeity);

                            var cards = await _repositoryCardsDashboard.FindByAsync(ActivityFilter.HalfYearly);
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
                                    TransCount = rubixTrasactions,
                                    UsersCount = rubixusers,
                                    CirculatingSupply = circulatingSupply
                                };

                                await _repositoryCardsDashboard.InsertAsync(new CardsDashboard()
                                {
                                    ActivityFilter = ActivityFilter.HalfYearly,
                                    EntityType = EntityType.Transactions,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(obj),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }
                        }
                        Console.WriteLine("Half year completed");
                        break;
                    case ActivityFilter.Yearly:
                        {
                            var rubixTokens = await _repositoryRubixToken.GetCountByFilterAsync(activeity);

                            var rubixTrasactions = await _repositoryRubixTransaction.GetCountByFilterAsync(activeity);

                            var cards = await _repositoryCardsDashboard.FindByAsync(ActivityFilter.Yearly);
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
                                    TransCount = rubixTrasactions,
                                    UsersCount = rubixusers,
                                    CirculatingSupply = circulatingSupply
                                };

                                await _repositoryCardsDashboard.InsertAsync(new CardsDashboard()
                                {
                                    ActivityFilter = ActivityFilter.Yearly,
                                    EntityType = EntityType.Transactions,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(obj),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }
                        }

                        Console.WriteLine("Year completed");
                        break;
                    case ActivityFilter.All:
                        {
                            var rubixTokens = await _repositoryRubixToken.GetCountByFilterAsync(activeity);

                            var rubixTrasactions = await _repositoryRubixTransaction.GetCountByFilterAsync(activeity);

                            var cards = await _repositoryCardsDashboard.FindByAsync(ActivityFilter.All);
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
                                    TransCount = rubixTrasactions,
                                    UsersCount = rubixusers,
                                    CirculatingSupply = circulatingSupply
                                };

                                await _repositoryCardsDashboard.InsertAsync(new CardsDashboard()
                                {
                                    ActivityFilter = ActivityFilter.All,
                                    EntityType = EntityType.Transactions,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(obj),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }

                        }
                        break;
                }

            }
            Console.WriteLine("*********** Cards Completed ****************************");
        }
    }
}
