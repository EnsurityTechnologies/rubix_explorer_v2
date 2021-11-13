using Newtonsoft.Json;
using Quartz;
using Rubix.API.Shared;
using Rubix.API.Shared.Common;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Enums;
using Rubix.API.Shared.Interfaces;
using Rubix.API.Shared.Repositories;
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
        private readonly IRepositoryRubixTransaction _repositoryRubixTransaction;
        private readonly IRepositoryRubixToken _repositoryRubixToken;

        public RubixDashboardJob(IRepositoryDashboard repositoryDashboard, IRepositoryRubixTransaction repositoryRubixTransaction, IRepositoryRubixToken repositoryRubixToken)
        {
            _repositoryDashboard = repositoryDashboard;
            _repositoryRubixTransaction = repositoryRubixTransaction;
            _repositoryRubixToken = repositoryRubixToken;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("*********************************************");

            //Transaction , Tokens Live charts data updates.

            var values = EnumUtil.GetValues<ActivityFilter>();
            foreach (var activeity in values)
            {
                switch(activeity)
                {
                    case ActivityFilter.Today:
                        {

                          
                            var transList = await _repositoryRubixTransaction.GetAllTodayRecords();



                            var tokensList =await _repositoryRubixToken.GetAllTodayRecords();



                            //Transactions
                            var trans = await _repositoryDashboard.FindByAsync(ActivityFilter.Today, EntityType.Transactions);
                            if (trans != null)
                            {
                                trans.Data = JsonConvert.SerializeObject(transList);
                                trans.LastModificationTime = DateTime.UtcNow;
                                await _repositoryDashboard.UpdateAsync(trans);
                            }
                            else
                            {
                                await _repositoryDashboard.InsertAsync(new Dashboard()
                                {
                                    ActivityFilter = ActivityFilter.Today,
                                    EntityType = EntityType.Transactions,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(transList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }


                            // Tokens

                            var tokens = await _repositoryDashboard.FindByAsync(ActivityFilter.Today, EntityType.Tokens);
                            if (tokens != null)
                            {
                                tokens.Data = JsonConvert.SerializeObject(tokensList);
                                tokens.LastModificationTime = DateTime.UtcNow;
                                await _repositoryDashboard.UpdateAsync(tokens);
                            }
                            else
                            {
                                await _repositoryDashboard.InsertAsync(new Dashboard()
                                {
                                    ActivityFilter = ActivityFilter.Today,
                                    EntityType = EntityType.Tokens,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(tokensList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }
                        }
                        break;
                    case ActivityFilter.Weekly:
                        {


                            //Transactions

                            List<Resultdto> transList = new List<Resultdto>();
                            List<Resultdto> tokensList = new List<Resultdto>();

                            for (int i = 1; i <= 7; i++)
                            {
                                var date = DateTime.Today.AddDays(-i);
                                var nextDate = date.AddDays(1);
                                var transCount = _repositoryRubixTransaction.GetAllAsync().Result.Where(x => x.CreationTime >= date && x.CreationTime < nextDate).Count();
                                transList.Add(new Resultdto() { 
                                         Key=date.Date.ToString("dd/MMM/yyyy"),
                                         Value= transCount
                                });

                                var tokensCount = _repositoryRubixToken.GetAllAsync().Result.Where(x => x.CreationTime >= date && x.CreationTime < nextDate).Count();
                                tokensList.Add(new Resultdto()
                                {
                                    Key = date.Date.ToString("dd/MMM/yyyy"),
                                    Value = tokensCount
                                });
                            }

                            //Transactions
                            var trans= await _repositoryDashboard.FindByAsync(ActivityFilter.Weekly, EntityType.Transactions);
                            if (trans != null)
                            {
                                trans.Data = JsonConvert.SerializeObject(transList);
                                trans.LastModificationTime = DateTime.UtcNow;
                                await _repositoryDashboard.UpdateAsync(trans);
                            }
                            else
                            {
                                await _repositoryDashboard.InsertAsync(new Dashboard()
                                {
                                    ActivityFilter = ActivityFilter.Weekly,
                                    EntityType = EntityType.Transactions,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(transList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }


                            // Tokens

                            var tokens = await _repositoryDashboard.FindByAsync(ActivityFilter.Weekly, EntityType.Tokens);
                            if (tokens != null)
                            {
                                tokens.Data = JsonConvert.SerializeObject(tokensList);
                                tokens.LastModificationTime = DateTime.UtcNow;
                                await _repositoryDashboard.UpdateAsync(tokens);
                            }
                            else
                            {
                                await _repositoryDashboard.InsertAsync(new Dashboard()
                                {
                                    ActivityFilter = ActivityFilter.Weekly,
                                    EntityType = EntityType.Tokens,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(tokensList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }
                        }

                        break;
                    case ActivityFilter.Monthly:
                        {
                            DateTime currentDate = DateTime.Today;
                            DateTime anotherDate = currentDate.AddMonths(-1);
                            DayOfWeek weekName = anotherDate.DayOfWeek;
                            int totalWeeksPerMonth = currentDate.WeekdayCount(anotherDate, weekName);

                            var tempDate = anotherDate;

                            //Transactions And Tokens

                            List<Resultdto> transList = new List<Resultdto>();
                            List<Resultdto> tokensList = new List<Resultdto>();

                            for (int i = 1; i <= totalWeeksPerMonth; i++)
                            {
                                if (i == 1)
                                {
                                    tempDate = anotherDate;
                                }
                                var WeekStartDate = tempDate;
                                var WeekEndDate = WeekStartDate.AddDays(7);

                                var transCount = _repositoryRubixTransaction.GetAllAsync().Result.Where(x => x.CreationTime >= WeekStartDate && x.CreationTime < WeekEndDate).Count();
                                
                                transList.Add(new Resultdto()
                                {
                                    Key = "Week "+i,
                                    Value = transCount
                                });

                                var tokensCount = _repositoryRubixToken.GetAllAsync().Result.Where(x => x.CreationTime >= WeekStartDate && x.CreationTime < WeekEndDate).Count();
                               
                                tokensList.Add(new Resultdto()
                                {
                                    Key = "Week " + i,
                                    Value = tokensCount
                                });

                                tempDate = WeekEndDate;
                            }

                            //Transaction
                            var trans = await _repositoryDashboard.FindByAsync(ActivityFilter.Monthly, EntityType.Transactions);
                            if (trans != null)
                            {
                                trans.Data = JsonConvert.SerializeObject(transList);
                                trans.LastModificationTime = DateTime.UtcNow;
                                await _repositoryDashboard.UpdateAsync(trans);
                            }
                            else
                            {
                                await _repositoryDashboard.InsertAsync(new Dashboard()
                                {
                                    ActivityFilter = ActivityFilter.Monthly,
                                    EntityType = EntityType.Transactions,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(transList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }

                            //Tokens
                            var tok = await _repositoryDashboard.FindByAsync(ActivityFilter.Monthly, EntityType.Tokens);
                            if (tok != null)
                            {
                                tok.Data = JsonConvert.SerializeObject(tokensList);
                                tok.LastModificationTime = DateTime.UtcNow;
                                await _repositoryDashboard.UpdateAsync(tok);
                            }
                            else
                            {
                                await _repositoryDashboard.InsertAsync(new Dashboard()
                                {
                                    ActivityFilter = ActivityFilter.Monthly,
                                    EntityType = EntityType.Tokens,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(tokensList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }
                        }
                        break;
                    case ActivityFilter.Quarterly:
                        {

                            int months = 3;
                            DateTime currentDate = DateTime.Now;
                            DateTime anotherMonth = currentDate.AddMonths(-months);
                            var tempMonth = anotherMonth;


                            List<Resultdto> transList = new List<Resultdto>();
                            List<Resultdto> tokensList = new List<Resultdto>();

                            for (int i = 1; i <= months ; i++)
                            {
                                if (i == 1)
                                {
                                    tempMonth = anotherMonth;
                                }
                                var MonthStartDate = tempMonth;
                                var MonthEndDate = MonthStartDate.AddMonths(1);

                                var transCount =  _repositoryRubixTransaction.GetAllAsync().Result.Where(x => x.CreationTime >= MonthStartDate && x.CreationTime < MonthEndDate).Count();
                                transList.Add(new Resultdto()
                                {
                                    Key = MonthEndDate.ToString("MMM"),
                                    Value = transCount
                                });

                                var tokensCount = _repositoryRubixToken.GetAllAsync().Result.Where(x => x.CreationTime >= MonthStartDate && x.CreationTime < MonthEndDate).Count();
                                tokensList.Add(new Resultdto()
                                {
                                    Key = MonthEndDate.ToString("MMM"),
                                    Value = tokensCount
                                });
                                tempMonth = MonthEndDate;
                            }



                            //Transaction
                            var trans = await _repositoryDashboard.FindByAsync(ActivityFilter.Quarterly, EntityType.Transactions);
                            if (trans != null)
                            {
                                trans.Data = JsonConvert.SerializeObject(transList);
                                trans.LastModificationTime = DateTime.UtcNow;
                                await _repositoryDashboard.UpdateAsync(trans);
                            }
                            else
                            {
                                await _repositoryDashboard.InsertAsync(new Dashboard()
                                {
                                    ActivityFilter = ActivityFilter.Quarterly,
                                    EntityType = EntityType.Transactions,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(transList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }

                            //Tokens
                            var tok = await _repositoryDashboard.FindByAsync(ActivityFilter.Quarterly, EntityType.Tokens);
                            if (tok != null)
                            {
                                tok.Data = JsonConvert.SerializeObject(tokensList);
                                tok.LastModificationTime = DateTime.UtcNow;
                                await _repositoryDashboard.UpdateAsync(tok);
                            }
                            else
                            {
                                await _repositoryDashboard.InsertAsync(new Dashboard()
                                {
                                    ActivityFilter = ActivityFilter.Quarterly,
                                    EntityType = EntityType.Tokens,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(tokensList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }
                        }
                        break;
                    case ActivityFilter.HalfYearly:
                        {
                            int months = 6;
                            DateTime currentDate = DateTime.Today;
                            DateTime anotherMonth = currentDate.AddMonths(-months);
                            var tempMonth = anotherMonth;


                            List<Resultdto> transList = new List<Resultdto>();
                            List<Resultdto> tokensList = new List<Resultdto>();

                            for (int i = 1; i <= months; i++)
                            {
                                if (i == 1)
                                {
                                    tempMonth = anotherMonth;
                                }
                                var MonthStartDate = tempMonth;
                                var MonthEndDate = MonthStartDate.AddMonths(1);

                                var transCount = _repositoryRubixTransaction.GetAllAsync().Result.Where(x => x.CreationTime >= MonthStartDate && x.CreationTime < MonthEndDate).Count();
                                transList.Add(new Resultdto()
                                {
                                    Key = MonthEndDate.ToString("MMM"),
                                    Value = transCount
                                });

                                var tokensCount = _repositoryRubixToken.GetAllAsync().Result.Where(x => x.CreationTime >= MonthStartDate && x.CreationTime < MonthEndDate).Count();
                                tokensList.Add(new Resultdto()
                                {
                                    Key = MonthEndDate.ToString("MMM"),
                                    Value = tokensCount
                                });
                                tempMonth = MonthEndDate;
                            }



                            //Transaction
                            var trans = await _repositoryDashboard.FindByAsync(ActivityFilter.HalfYearly, EntityType.Transactions);
                            if (trans != null)
                            {
                                trans.Data = JsonConvert.SerializeObject(transList);
                                trans.LastModificationTime = DateTime.UtcNow;
                                await _repositoryDashboard.UpdateAsync(trans);
                            }
                            else
                            {
                                await _repositoryDashboard.InsertAsync(new Dashboard()
                                {
                                    ActivityFilter = ActivityFilter.HalfYearly,
                                    EntityType = EntityType.Transactions,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(transList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }

                            //Tokens
                            var tok = await _repositoryDashboard.FindByAsync(ActivityFilter.HalfYearly, EntityType.Tokens);
                            if (tok != null)
                            {
                                tok.Data = JsonConvert.SerializeObject(tokensList);
                                tok.LastModificationTime = DateTime.UtcNow;
                                await _repositoryDashboard.UpdateAsync(tok);
                            }
                            else
                            {
                                await _repositoryDashboard.InsertAsync(new Dashboard()
                                {
                                    ActivityFilter = ActivityFilter.HalfYearly,
                                    EntityType = EntityType.Tokens,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(tokensList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }
                        }
                        break;
                    case ActivityFilter.Yearly:
                        {
                            int months = 12;
                            DateTime currentDate = DateTime.Today;
                            DateTime anotherMonth = currentDate.AddMonths(-months);
                            var tempMonth = anotherMonth;


                            List<Resultdto> transList = new List<Resultdto>();
                            List<Resultdto> tokensList = new List<Resultdto>();

                            for (int i = 1; i <= months; i++)
                            {
                                if (i == 1)
                                {
                                    tempMonth = anotherMonth;
                                }
                                var MonthStartDate = tempMonth;
                                var MonthEndDate = MonthStartDate.AddMonths(1);

                                var transCount = _repositoryRubixTransaction.GetAllAsync().Result.Where(x => x.CreationTime >= MonthStartDate && x.CreationTime < MonthEndDate).Count();
                                transList.Add(new Resultdto()
                                {
                                    Key = MonthEndDate.ToString("MMM"),
                                    Value = transCount
                                });

                                var tokensCount = _repositoryRubixToken.GetAllAsync().Result.Where(x => x.CreationTime >= MonthStartDate && x.CreationTime < MonthEndDate).Count();
                                tokensList.Add(new Resultdto()
                                {
                                    Key = MonthEndDate.ToString("MMM"),
                                    Value = tokensCount
                                });
                                tempMonth = MonthEndDate;
                            }



                            //Transaction
                            var trans = await _repositoryDashboard.FindByAsync(ActivityFilter.Yearly, EntityType.Transactions);
                            if (trans != null)
                            {
                                trans.Data = JsonConvert.SerializeObject(transList);
                                trans.LastModificationTime = DateTime.UtcNow;
                                await _repositoryDashboard.UpdateAsync(trans);
                            }
                            else
                            {
                                await _repositoryDashboard.InsertAsync(new Dashboard()
                                {
                                    ActivityFilter = ActivityFilter.Yearly,
                                    EntityType = EntityType.Transactions,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(transList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }

                            //Tokens
                            var tok = await _repositoryDashboard.FindByAsync(ActivityFilter.Yearly, EntityType.Tokens);
                            if (tok != null)
                            {
                                tok.Data = JsonConvert.SerializeObject(tokensList);
                                tok.LastModificationTime = DateTime.UtcNow;
                                await _repositoryDashboard.UpdateAsync(tok);
                            }
                            else
                            {
                                await _repositoryDashboard.InsertAsync(new Dashboard()
                                {
                                    ActivityFilter = ActivityFilter.Yearly,
                                    EntityType = EntityType.Tokens,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(tokensList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }
                        }
                        break;
                    case ActivityFilter.All:
                        {
                            int start = 2018;
                            int end = DateTime.UtcNow.Year;
                            int yearsGap = end - start;
                            DateTime currentYearDate = DateTime.Today;
                            DateTime endYear = currentYearDate.AddYears(-yearsGap);

                            var tempYear = endYear;


                            List<Resultdto> transList = new List<Resultdto>();
                            List<Resultdto> tokensList = new List<Resultdto>();


                            for (int i = 1; i <= yearsGap; i++)
                            {
                                if (i == 1)
                                {
                                    tempYear = endYear;
                                }

                                var YearStartDate = tempYear;
                                var YearEndDate = YearStartDate.AddYears(1);

                                var transCount = _repositoryRubixTransaction.GetAllAsync().Result.Where(x => x.CreationTime >= YearStartDate && x.CreationTime < YearEndDate).Count();

                                transList.Add(new Resultdto()
                                {
                                    Key = YearEndDate.Year.ToString(),
                                    Value = transCount
                                });

                                var tokensCount = _repositoryRubixToken.GetAllAsync().Result.Where(x => x.CreationTime >= YearStartDate && x.CreationTime < YearEndDate).Count();

                                tokensList.Add(new Resultdto()
                                {
                                    Key = YearEndDate.Year.ToString(),
                                    Value = tokensCount
                                });
                                tempYear = YearEndDate;
                            }



                            //Transaction
                            var trans = await _repositoryDashboard.FindByAsync(ActivityFilter.All, EntityType.Transactions);
                            if (trans != null)
                            {
                                trans.Data = JsonConvert.SerializeObject(transList);
                                trans.LastModificationTime = DateTime.UtcNow;
                                await _repositoryDashboard.UpdateAsync(trans);
                            }
                            else
                            {
                                await _repositoryDashboard.InsertAsync(new Dashboard()
                                {
                                    ActivityFilter = ActivityFilter.All,
                                    EntityType = EntityType.Transactions,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(transList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }

                            //Tokens
                            var tok = await _repositoryDashboard.FindByAsync(ActivityFilter.All, EntityType.Tokens);
                            if (tok != null)
                            {
                                tok.Data = JsonConvert.SerializeObject(tokensList);
                                tok.LastModificationTime = DateTime.UtcNow;
                                await _repositoryDashboard.UpdateAsync(tok);
                            }
                            else
                            {
                                await _repositoryDashboard.InsertAsync(new Dashboard()
                                {
                                    ActivityFilter = ActivityFilter.All,
                                    EntityType = EntityType.Tokens,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(tokensList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }

                        }
                        break;
                }
   
            }

            Console.WriteLine("Completed");
        }
    }
}
