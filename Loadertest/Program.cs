using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Rubix.API.Shared;
using Rubix.API.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loadertest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var client = new MongoClient("mongodb+srv://admin:DtfeJS0G5vfUtNWI@cluster0.peyce.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");

                IMongoDatabase db = client.GetDatabase("rubixDb");

                var collection = db.GetCollection<RubixTransaction>("_transactions");


                var today = DateTime.Today;


                //var month1 = DateTime.Today.AddMonths(-1);
                //var month2 = DateTime.Today.AddMonths(-2);
                //var month3 = DateTime.Today.AddMonths(-3);

                //var month1results = await collection.AsQueryable().Where(x => x.CreationTime > month1).CountAsync();

                //Console.WriteLine(month1results); 


                //var month2results = await collection.AsQueryable().Where(x => x.CreationTime > month2).CountAsync();

                //Console.WriteLine(month2results - month1results);


                //var month3results = await collection.AsQueryable().Where(x => x.CreationTime > month3).CountAsync();

                //Console.WriteLine(month3results - month2results);

                //Console.WriteLine(month3results);

                // Days - Week
               
                
                //long total = 0;
                //for(int i=1; i <= 7;i ++)
                //{
                //    var date = DateTime.Today.AddDays(-i);
                //    var nextDate = date.AddDays(1);
                //    var dayCount = await collection.AsQueryable().Where(x => x.CreationTime >= date && x.CreationTime < nextDate).CountAsync();
                //    total= total+dayCount;
                //}
                //Console.WriteLine("**************");
                //Console.WriteLine(total);

                //var WeekRecords=  DateTime.Today.AddDays(-7);
                //var dayCounts = await collection.AsQueryable().Where(x => x.CreationTime <= DateTime.Today && x.CreationTime >= WeekRecords).CountAsync();
                //Console.WriteLine(dayCounts);
                //Console.WriteLine("**************");


                //Weeks - Month records

                DateTime currentDate = DateTime.Today;
                DateTime anotherDate = currentDate.AddMonths(-1);
                DayOfWeek weekName = anotherDate.DayOfWeek;
                int totalWeeksPerMonth = currentDate.WeekdayCount(anotherDate, weekName);
                Console.WriteLine();

                int monthCount = 0;
                var tempDate = anotherDate;
                for (int i = 1; i <= totalWeeksPerMonth; i++)
                {
                     Console.WriteLine("Week:"+i);


                        if(i==1)
                        {
                         tempDate = anotherDate;
                        }

                        var WeekStartDate= tempDate;
                        var WeekEndDate = WeekStartDate.AddDays(7);

                        var data= await collection.AsQueryable().Where(x => x.CreationTime >= WeekStartDate && x.CreationTime < WeekEndDate).CountAsync();
                        Console.WriteLine(data);

                        monthCount = monthCount + data;
                        tempDate = WeekEndDate;  
                }

                Console.WriteLine(monthCount);

                var totalCount = await collection.AsQueryable().Where(x => x.CreationTime <= currentDate && x.CreationTime >= anotherDate).CountAsync();
                Console.WriteLine(totalCount);
                Console.WriteLine("**************");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
