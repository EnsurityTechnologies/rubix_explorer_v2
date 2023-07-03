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



                #region today Records

                //List<Resultdto> result = new List<Resultdto>();

                //var collection = db.GetCollection<BsonDocument>("_transactions");
                //// Get today's date
                //DateTime today = DateTime.Now.Date;

                //// Iterate through hours from 0 to 23
                //for (int hour = 0; hour <= 23; hour++)
                //{
                //    // Get the start and end time for the current hour
                //    DateTime startTime = today.AddHours(hour);
                //    DateTime endTime = startTime.AddHours(1);

                //    // Create the filter to find transactions within the current hour
                //    var filter = Builders<BsonDocument>.Filter.Gte("CreationTime", startTime) &
                //                 Builders<BsonDocument>.Filter.Lt("CreationTime", endTime);

                //    // Count the number of transactions in the current hour
                //    long count = collection.CountDocuments(filter);

                //    string label = (hour < 12) ? "AM" : "PM";
                //    int displayHour = (hour == 0 || hour == 12) ? 12 : hour % 12;

                //    result.Add(new Resultdto()
                //    {
                //        Key = $"{displayHour} {label}",
                //        Value = count,
                //    });
                //}

                #endregion



                //            #region    Month Records
                //            List<Resultdto> result = new List<Resultdto>();

                //            var collection = db.GetCollection<BsonDocument>("_transactions");

                //            var startDate = DateTime.Now.AddYears(-1);
                //            var endDate = DateTime.Now;

                //            var pipeline = new BsonDocument[]
                //            {
                //                  new BsonDocument("$match", new BsonDocument
                //                   {{ "CreationTime", new BsonDocument
                //               {
                //              { "$gte", startDate },

                //              { "$lte", endDate }
                //                  }
                //               }
                //              }),


                //// Group by the date and count the records

                //new BsonDocument("$group", new BsonDocument

                //{

                //    { "_id", new BsonDocument("$dateToString", new BsonDocument

                //        {

                //            { "format", "%Y-%m-%d" },

                //            { "date", "$CreationTime" }

                //        })

                //    },

                //    { "count", new BsonDocument("$sum", 1) }

                //})

                //            };



                //            // Execute the aggregation pipeline

                //            var data = collection.Aggregate<BsonDocument>(pipeline).ToList();





                //            var dateCounts = data.ToDictionary(x => x["_id"].AsString, x => x["count"].AsInt32);

                //            var currentDate = startDate;
                //            while (currentDate <= endDate)
                //            {
                //                var currentDateStr = currentDate.ToString("yyyy-MM-dd");
                //                var count = dateCounts.ContainsKey(currentDateStr) ? dateCounts[currentDateStr] : 0;
                //                Console.WriteLine($"Date: {currentDateStr}, Count: {count}");
                //                result.Add(new Resultdto()
                //                {
                //                    Key = currentDateStr,
                //                    Value = count,
                //                });

                //                currentDate = currentDate.AddDays(1);
                //            }

                //            #endregion


                #region   Last One Year Records
                //var collection = db.GetCollection<BsonDocument>("_transactions");
                var startDate = DateTime.UtcNow.AddYears(-1); // Start date for the last one year
                var endDate = DateTime.UtcNow; // End date (current date)

                var matchStage = new BsonDocument("$match", new BsonDocument
                {
                    { "CreationTime", new BsonDocument { { "$gte", startDate }, { "$lte", endDate } } }
                });

                var projectStage = new BsonDocument("$project", new BsonDocument
                {
                    { "Month", new BsonDocument("$dateToString", new BsonDocument { { "format", "%Y-%m" }, { "date", "$CreationTime" } }) },
                    { "Count", 1 }
                });

                        var groupStage = new BsonDocument("$group", new BsonDocument
                {
                    { "_id", "$Month" },
                    { "Count", new BsonDocument("$sum", 1) }
                });


                        var sortStage = new BsonDocument("$sort", new BsonDocument
                {
                    { "_id", 1 } // 1 for ascending order, -1 for descending order
                });
                var pipeline = new List<BsonDocument> { matchStage, projectStage, groupStage, sortStage };
                var collection = db.GetCollection<BsonDocument>("_transactions");
                var result = collection.Aggregate<BsonDocument>(pipeline);

                var monthlyCounts = new List<MonthlyCount>();

                foreach (var document in result.ToList())
                {
                    var month = DateTime.Parse(document.GetValue("_id").AsString);
                    var count = document.GetValue("Count").ToInt32();

                    var monthlyCount = new MonthlyCount
                    {
                        Month = month,
                        Count = count
                    };

                    monthlyCounts.Add(monthlyCount);
                }

                // Printing the monthly counts
                foreach (var monthlyCount in monthlyCounts)
                {
                    Console.WriteLine($"{monthlyCount.Month:MMMM-yyyy}: {monthlyCount.Count}");
                }
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

    public class MonthlyCount
    {
        public DateTime Month { get; set; }
        public int Count { get; set; }
    }
}
