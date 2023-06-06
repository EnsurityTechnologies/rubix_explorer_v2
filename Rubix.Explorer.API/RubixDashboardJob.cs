using MongoDB.Bson;
using MongoDB.Driver;
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
        private MongoClient client = null;
        private IMongoDatabase db = null;
        string _transCollenctioName = "_transactions";
        string _tokensCollectionName = "_tokens";

        public RubixDashboardJob(IRepositoryDashboard repositoryDashboard)
        {
            _repositoryDashboard = repositoryDashboard;
            var login = "admin";
            var password = Uri.EscapeDataString("IjzUmspU8yDwg5MW");
            var server = "cluster0.jeaxq.mongodb.net";
            client = new MongoClient($"mongodb+srv://{login}:{password}@{server}/rubixDb?retryWrites=true&w=majority");

            db = client.GetDatabase("rubixDb");
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
                            //Transactions
                            var transList = await getTodayRecords(_transCollenctioName);
                            var tokensList = await getTodayRecords(_tokensCollectionName);
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
                            Console.WriteLine("dash today completed");
                        }
                        break;
                    case ActivityFilter.Week:
                        {
                            var transList = await GetLastWeekRecords(_transCollenctioName);
                            var tokensList = await GetLastWeekRecords(_tokensCollectionName);
                            var trans = await _repositoryDashboard.FindByAsync(ActivityFilter.Week, EntityType.Transactions);
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
                                    ActivityFilter = ActivityFilter.Week,
                                    EntityType = EntityType.Transactions,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(transList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }

                            // Tokens

                            var tokens = await _repositoryDashboard.FindByAsync(ActivityFilter.Week, EntityType.Tokens);
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
                            Console.WriteLine("dash Weekly completed");
                            break;
                        }
                    case ActivityFilter.Month:
                        {
                            var transList = await GetMonthRecord(_transCollenctioName);
                            var tokensList = await GetMonthRecord(_tokensCollectionName);
                            var trans = await _repositoryDashboard.FindByAsync(ActivityFilter.Month, EntityType.Transactions);
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
                                    ActivityFilter = ActivityFilter.Month,
                                    EntityType = EntityType.Transactions,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(transList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }

                            // Tokens

                            var tokens = await _repositoryDashboard.FindByAsync(ActivityFilter.Month, EntityType.Tokens);
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
                                    ActivityFilter = ActivityFilter.Month,
                                    EntityType = EntityType.Tokens,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(tokensList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }
                            Console.WriteLine("dash Monthly completed");
                            break;
                        }
                    case ActivityFilter.Year:
                        {
                            var transList = await GetLastYearRecords(_transCollenctioName);
                            var tokensList = await GetLastYearRecords(_tokensCollectionName);
                            var trans = await _repositoryDashboard.FindByAsync(ActivityFilter.Year, EntityType.Transactions);
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
                                    ActivityFilter = ActivityFilter.Year,
                                    EntityType = EntityType.Transactions,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(transList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }

                            // Tokens

                            var tokens = await _repositoryDashboard.FindByAsync(ActivityFilter.Year, EntityType.Tokens);
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
                                    ActivityFilter = ActivityFilter.Year,
                                    EntityType = EntityType.Tokens,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(tokensList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }
                            Console.WriteLine("dash Quarter completed");
                            break;
                        }
                    case ActivityFilter.All:
                        {

                            var transList = await GetAllRecordsAsync(_transCollenctioName);
                            var tokensList = await GetAllRecordsAsync(_tokensCollectionName);
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

                            // Tokens

                            var tokens = await _repositoryDashboard.FindByAsync(ActivityFilter.All, EntityType.Tokens);
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
                                    ActivityFilter = ActivityFilter.All,
                                    EntityType = EntityType.Tokens,
                                    CreationTime = DateTime.UtcNow,
                                    Data = JsonConvert.SerializeObject(tokensList),
                                    LastModificationTime = DateTime.UtcNow
                                });
                            }
                            Console.WriteLine("dash All completed");
                            break;
                        }
                }
            }
            Console.WriteLine("****************Dashboard Completed*****************");
        }
        #region    Today

        private async Task<List<Resultdto>> getTodayRecords(string collectionName)
        {
            List<Resultdto> result = new List<Resultdto>();

            var collection = db.GetCollection<BsonDocument>(collectionName);
            // Get today's date
            DateTime today = DateTime.Now.Date;

            // Iterate through hours from 0 to 23
            for (int hour = 0; hour <= 23; hour++)
            {
                // Get the start and end time for the current hour
                DateTime startTime = today.AddHours(hour);
                DateTime endTime = startTime.AddHours(1);

                // Create the filter to find transactions within the current hour
                var filter = Builders<BsonDocument>.Filter.Gte("CreationTime", startTime) &
                             Builders<BsonDocument>.Filter.Lt("CreationTime", endTime);

                // Count the number of transactions in the current hour
                long count = collection.CountDocuments(filter);

                string label = (hour < 12) ? "AM" : "PM";
                int displayHour = (hour == 0 || hour == 12) ? 12 : hour % 12;

                result.Add(new Resultdto()
                {
                    Key = $"{displayHour} {label}",
                    Value = count,
                });
            }
            return await Task.FromResult(result);
        }
        #endregion

        #region   Last 7 Days

        public async Task<List<Resultdto>> GetLastWeekRecords(string collectionName)
        {
            List<Resultdto> result = new List<Resultdto>();

            var collection = db.GetCollection<BsonDocument>(collectionName);

            var lastWeekStartDate = DateTime.Now.AddDays(-7).Date;
            var lastWeekEndDate = DateTime.Now.Date;

            var filter = Builders<BsonDocument>.Filter.Gte("CreationTime", lastWeekStartDate) & Builders<BsonDocument>.Filter.Lte("CreationTime", lastWeekEndDate);
            var group = new BsonDocument
            {
                { "_id", new BsonDocument("$month", "$CreationTime") },
                { "count", new BsonDocument("$sum", 1) }
            };
            var aggregation = collection.Aggregate()
                .Match(filter)
                .Group(group);

            var results = aggregation.ToList();
            var dateCounts = results.ToDictionary(x => x["_id"].AsString, x => x["count"].AsInt32);

            var currentDate = lastWeekStartDate;
            while (currentDate <= lastWeekEndDate)
            {
                var currentDateStr = currentDate.ToString("yyyy-MM-dd");
                var count = dateCounts.ContainsKey(currentDateStr) ? dateCounts[currentDateStr] : 0;
                Console.WriteLine($"Date: {currentDateStr}, Count: {count}");
                result.Add(new Resultdto()
                {
                    Key = currentDateStr,
                    Value = count,
                });

                currentDate = currentDate.AddDays(1);
            }
            return await Task.FromResult(result);
        }
        #endregion

        #region    Month Records
        private async Task<List<Resultdto>> GetMonthRecord(string collectionName)
        {
            List<Resultdto> resultdtos = new List<Resultdto>();
            var collection = db.GetCollection<BsonDocument>(collectionName);

            var lastMonthStartDate = DateTime.Now.AddDays(-30).Date;
            var lastMonthEndDate = DateTime.Now.Date;

            var filter = Builders<BsonDocument>.Filter.Gte("CreationTime", lastMonthStartDate) & Builders<BsonDocument>.Filter.Lte("CreationTime", lastMonthEndDate);
            var group = new BsonDocument
            {
                { "_id", new BsonDocument("$week", "$CreationTime") },
                { "count", new BsonDocument("$sum", 1) }
            };
            var aggregation = collection.Aggregate()
                .Match(filter)
                .Group(group);

            var results = aggregation.ToList();
            var weekCounts = new Dictionary<int, int>();

            foreach (var result in results)
            {
                var weekNumber = result["_id"].AsInt32;
                var count = result["count"].AsInt32;
                weekCounts[weekNumber] = count;
            }

            for (int week = 1; week <= 4; week++)
            {
                var count = weekCounts.ContainsKey(week) ? weekCounts[week] : 0;
                Console.WriteLine($"Week{week}: Count: {count}");
                resultdtos.Add(new Resultdto()
                {
                    Key = $"Week{week}",
                    Value = count
                });
            }
            return await Task.FromResult(resultdtos);
        }
        #endregion


        #region   Last One Year Records

        private async Task<List<Resultdto>> GetLastYearRecords(string collectionName)
        {

            List<Resultdto> resultdtos = new List<Resultdto>();
            var collection = db.GetCollection<BsonDocument>(collectionName);


            var lastYearStartDate = DateTime.Now.AddYears(-1).Date;
            var lastYearEndDate = DateTime.Now.Date;

            var filter = Builders<BsonDocument>.Filter.Gte("CreationTime", lastYearStartDate) & Builders<BsonDocument>.Filter.Lte("CreationTime", lastYearEndDate);
            var group = new BsonDocument
            {
                { "_id", new BsonDocument("$month", "$CreationTime") },
                { "count", new BsonDocument("$sum", 1) }
            };
            var aggregation = collection.Aggregate()
                .Match(filter)
                .Group(group);

            var results = aggregation.ToList();
            var monthCounts = new Dictionary<int, int>();

            foreach (var result in results)
            {
                var monthNumber = result["_id"].AsInt32;
                var count = result["count"].AsInt32;
                monthCounts[monthNumber] = count;
            }

            var allMonths = Enumerable.Range(1, 12);
            var dateTimeFormatInfo = new DateTimeFormatInfo();
            foreach (var month in allMonths)
            {
                var monthName = dateTimeFormatInfo.GetMonthName(month);
                var count = monthCounts.ContainsKey(month) ? monthCounts[month] : 0;
                Console.WriteLine($"Month: {monthName}, Count: {count}");

                resultdtos.Add(new Resultdto()
                {
                    Key = monthName,
                    Value = count
                });
            }
            return await Task.FromResult(resultdtos);
        }
        #endregion

        #region   All Records


        private async Task<List<Resultdto>> GetAllRecordsAsync(string collectionName)
        {

            List<Resultdto> resultdtos = new List<Resultdto>();
            var collection = db.GetCollection<BsonDocument>(collectionName);

            var group = new BsonDocument
            {
                { "_id", new BsonDocument("$year", "$CreationTime") },
                { "count", new BsonDocument("$sum", 1) }
            };
            var aggregation = collection.Aggregate()
                .Group(group);

            var results = aggregation.ToList();
            var yearCounts = new Dictionary<int, int>();

            foreach (var result in results)
            {
                var year = result["_id"].AsInt32;
                var count = result["count"].AsInt32;
                yearCounts[year] = count;
            }

            foreach (var year in yearCounts.Keys)
            {
                var count = yearCounts[year];
                Console.WriteLine($"Year: {year}, Count: {count}");

                resultdtos.Add(new Resultdto()
                {
                    Key = year.ToString(),
                    Value = count
                });
            }
            return await Task.FromResult(resultdtos);
        }
        #endregion
    }
}
