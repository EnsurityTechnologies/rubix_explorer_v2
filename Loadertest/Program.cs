using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Rubix.API.Shared;
using Rubix.API.Shared.Common;
using Rubix.API.Shared.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Loadertest
{
    class Program
    {

        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        private static string CalculateDifference(DateTime fromData, DateTime toData)
        {
            TimeSpan ts = toData - fromData;
            string diff = string.Format("{0} hours, {1} minutes", ts.Hours, ts.Minutes);

            return diff;
        }

        private MongoClient client = null;
        private IMongoDatabase db = null;

        public static void Main(string[] args)
        {
            try
            {
                var login = "admin";
                var password = Uri.EscapeDataString("IjzUmspU8yDwg5MW");
                var server = "cluster0.jeaxq.mongodb.net";
                MongoClient client = new MongoClient($"mongodb+srv://{login}:{password}@{server}/rubixDb?retryWrites=true&w=majority");

                IMongoDatabase db = client.GetDatabase("rubixDb");

                #region    Month Records
                var collection = db.GetCollection<BsonDocument>("_transactions");

                var today = DateTime.Today;
                var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                var filter = Builders<BsonDocument>.Filter.Gte("CreationTime", firstDayOfMonth) & Builders<BsonDocument>.Filter.Lte("CreationTime", lastDayOfMonth);

                // Group by week number and count records
                var groupStage = new BsonDocument
                {
                    { "_id", new BsonDocument { { "Week", new BsonDocument("$week", "$CreationTime") }, { "Year", new BsonDocument("$year", "$CreationTime") } } },
                    { "Count", new BsonDocument("$sum", 1) }
                };

                var sortStage = new BsonDocument
                {
                    { "_id.Week", 1 }
                };

                var aggregation = collection.Aggregate()
                    .Match(filter)
                    .Group(groupStage)
                    .Sort(sortStage);

                // Execute the aggregation and retrieve the results
                var results = aggregation.ToList();

                // Display the count of records for each week
                foreach (var result in results)
                {
                    var weekNumber = result["_id"]["Week"].AsInt32;
                    var year = result["_id"]["Year"].AsInt32;
                    var count = result["Count"].AsInt32;

                    var weekLabel = GetWeekLabel(weekNumber, year);
                    Console.WriteLine($"{weekLabel}: {count} records");
                }

                #endregion


                #region   Last One Year Records
                //var collection = db.GetCollection<BsonDocument>("_transactions");


                //var lastYearStartDate = DateTime.Now.AddYears(-1).Date;
                //var lastYearEndDate = DateTime.Now.Date;

                //var filter = Builders<BsonDocument>.Filter.Gte("CreationTime", lastYearStartDate) & Builders<BsonDocument>.Filter.Lte("CreationTime", lastYearEndDate);
                //var group = new BsonDocument
                //{
                //    { "_id", new BsonDocument("$month", "$CreationTime") },
                //    { "count", new BsonDocument("$sum", 1) }
                //};
                //var aggregation = collection.Aggregate()
                //    .Match(filter)
                //    .Group(group);

                //var results = aggregation.ToList();
                //var monthCounts = new Dictionary<int, int>();

                //foreach (var result in results)
                //{
                //    var monthNumber = result["_id"].AsInt32;
                //    var count = result["count"].AsInt32;
                //    monthCounts[monthNumber] = count;
                //}

                //var allMonths = Enumerable.Range(1, 12);
                //var dateTimeFormatInfo = new DateTimeFormatInfo();
                //foreach (var month in allMonths)
                //{
                //    var monthName = dateTimeFormatInfo.GetMonthName(month);
                //    var count = monthCounts.ContainsKey(month) ? monthCounts[month] : 0;
                //    Console.WriteLine($"Month: {monthName}, Count: {count}");
                //}
                #endregion


                #region   All Records

                //var collection = db.GetCollection<BsonDocument>("_tokens");

                //var group = new BsonDocument
                //{
                //    { "_id", new BsonDocument("$year", "$CreationTime") },
                //    { "count", new BsonDocument("$sum", 1) }
                //};
                //var aggregation = collection.Aggregate()
                //    .Group(group);

                //var results = aggregation.ToList();
                //var yearCounts = new Dictionary<int, int>();

                //foreach (var result in results)
                //{
                //    var year = result["_id"].AsInt32;
                //    var count = result["count"].AsInt32;
                //    yearCounts[year] = count;
                //}

                //foreach (var year in yearCounts.Keys)
                //{
                //    var count = yearCounts[year];
                //    Console.WriteLine($"Year: {year}, Count: {count}");
                //}
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        #region    Today

        private List<Resultdto> getTodayRecords(string collectionName)
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

                result.Add(new Resultdto() { 
                 Key=$"{displayHour} {label}",
                 Value=count,
                });
            }
            return result;
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
            return result;
        }
        #endregion
        private static string GetWeekLabel(int weekNumber, int year)
        {
            var startDate = ISOWeek.ToDateTime(year, weekNumber, DayOfWeek.Monday);
            var endDate = startDate.AddDays(6);
            return $"{startDate:MMM dd} - {endDate:MMM dd}";
        }
    }
   

    public class Resultdto
    {
        public string Key { get; set; }

        public long Value { get; set; }
    }
}
