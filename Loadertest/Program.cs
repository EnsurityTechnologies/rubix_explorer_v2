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
                var login = "admin";
                var password = Uri.EscapeDataString("IjzUmspU8yDwg5MW");
                var server = "cluster0.jeaxq.mongodb.net";
                var client= new MongoClient($"mongodb+srv://{login}:{password}@{server}/rubixDb?retryWrites=true&w=majority");

                IMongoDatabase db = client.GetDatabase("rubixDb");

                var collection = db.GetCollection<RubixTransaction>("_transactions");


                // 7 Days - Week   


                //long total = 0;


                //var todayNow = DateTime.Now;
                var today = DateTime.Today;

                var start = Convert.ToDateTime(today).AddHours(24).AddSeconds(-1).ToString("dd/MM/yyyy hh:mm:ss tt");








                //var test = DateTime.Today.AddDays(-6).ToString("dd/MM/yyyy hh:mm:ss tt");

                //for (int i = 1; i <= 7; i++)
                //{

                //    var end = Convert.ToDateTime(test).AddDays(i).AddSeconds(-1);

                //    var start = Convert.ToDateTime(end).AddSeconds(1).AddHours(-24).ToString("dd/MM/yyyy hh:mm:ss tt");

                //    var vuu = Convert.ToDateTime(start);

                //    //Console.WriteLine("**************");
                //    Console.WriteLine(start);
                //    Console.WriteLine(end);
                //    //Console.WriteLine("**************");
                //    //var date = test.ToString("dd/MM/yyyy hh:mm:ss");
                //    //var nextDate = Convert.ToDateTime(date).AddDays(1);
                //    var dayCount = await collection.AsQueryable().Where(x => x.CreationTime.Value >= vuu && x.CreationTime.Value <= end).CountAsync();

                //   // Console.WriteLine(start);

                //    Console.WriteLine(dayCount);
                //}
                //Console.WriteLine("**************");
                //Console.WriteLine(total);

                //var WeekRecords=  DateTime.Today.AddDays(-7);
                //var dayCounts = await collection.AsQueryable().Where(x => x.CreationTime <= DateTime.Today && x.CreationTime >= WeekRecords).CountAsync();
                //Console.WriteLine(dayCounts);
                //Console.WriteLine("**************");








                // Start Weeks - Month records    **************************************

                //DateTime currentDate = DateTime.Today;
                //DateTime anotherDate = currentDate.AddMonths(-1);
                //DayOfWeek weekName = anotherDate.DayOfWeek;
                //int totalWeeksPerMonth = currentDate.WeekdayCount(anotherDate, weekName);
                //Console.WriteLine();

                //int monthCount = 0;
                //var tempDate = anotherDate;
                //for (int i = 1; i <= totalWeeksPerMonth; i++)
                //{
                //     Console.WriteLine("Week:"+i);


                //        if(i==1)
                //        {
                //         tempDate = anotherDate;
                //        }

                //        var WeekStartDate= tempDate;
                //        var WeekEndDate = WeekStartDate.AddDays(7);

                //        var data= await collection.AsQueryable().Where(x => x.CreationTime >= WeekStartDate && x.CreationTime < WeekEndDate).CountAsync();
                //        Console.WriteLine(data);

                //        monthCount = monthCount + data;
                //        tempDate = WeekEndDate;  
                //}

                //Console.WriteLine(monthCount);

                //var totalCount = await collection.AsQueryable().Where(x => x.CreationTime <= currentDate && x.CreationTime >= anotherDate).CountAsync();
                //Console.WriteLine(totalCount);
                //Console.WriteLine("**************");

                // End Weeks - Month records    **************************************









                // Start 3 Months - Qauterly records    **************************************

                //int totalMonthPerQuarter = 3;
                //DateTime currentDate = DateTime.Today; 
                //DateTime anotherMonth = currentDate.AddMonths(-totalMonthPerQuarter);


                //Console.WriteLine();

                //int monthCount = 0;
                //var tempMonth = anotherMonth;
                //for (int i = 1; i <= totalMonthPerQuarter; i++)
                //{
                //    Console.WriteLine("Month:" + i);


                //    if (i == 1)
                //    {
                //        tempMonth = anotherMonth;
                //    }

                //    var MonthStartDate = tempMonth;
                //    var MonthEndDate = MonthStartDate.AddMonths(1);

                //    var data = await collection.AsQueryable().Where(x => x.CreationTime >= MonthStartDate && x.CreationTime < MonthEndDate).CountAsync();
                //    Console.WriteLine(data);

                //    monthCount = monthCount + data;
                //    tempMonth = MonthEndDate;
                //}


                //Console.WriteLine("**************");

                //Console.WriteLine(monthCount);

                //var totalCount = await collection.AsQueryable().Where(x => x.CreationTime <= currentDate && x.CreationTime >= anotherMonth).CountAsync();
                //Console.WriteLine(totalCount);
                //Console.WriteLine("**************");

                // End Weeks - Month records    **************************************









                // Start 6 Months - Half Yearly records    **************************************

                //int totalMonthPerQuarter = 6;
                //DateTime currentDate = DateTime.Today;
                //DateTime anotherMonth = currentDate.AddMonths(-totalMonthPerQuarter);


                //Console.WriteLine();

                //int monthCount = 0;
                //var tempMonth = anotherMonth;
                //for (int i = 1; i <= totalMonthPerQuarter; i++)
                //{
                //    Console.WriteLine("Month:" + i);


                //    if (i == 1)
                //    {
                //        tempMonth = anotherMonth;
                //    }

                //    var MonthStartDate = tempMonth;
                //    var MonthEndDate = MonthStartDate.AddMonths(1);

                //    var data = await collection.AsQueryable().Where(x => x.CreationTime >= MonthStartDate && x.CreationTime < MonthEndDate).CountAsync();
                //    Console.WriteLine(data);

                //    monthCount = monthCount + data;
                //    tempMonth = MonthEndDate;
                //}


                //Console.WriteLine("**************");

                //Console.WriteLine(monthCount);

                //var totalCount = await collection.AsQueryable().Where(x => x.CreationTime <= currentDate && x.CreationTime >= anotherMonth).CountAsync();
                //Console.WriteLine(totalCount);
                //Console.WriteLine("**************");

                // End Weeks - Month records    **************************************














                // Start 12 Months - Yearly records    **************************************

                //int totalMonthPerQuarter = 6;
                //DateTime currentDate = DateTime.Today;
                //DateTime anotherMonth = currentDate.AddMonths(-totalMonthPerQuarter);


                //Console.WriteLine();

                //int monthCount = 0;
                //var tempMonth = anotherMonth;
                //for (int i = 1; i <= totalMonthPerQuarter; i++)
                //{
                //    Console.WriteLine("Month:" + i);


                //    if (i == 1)
                //    {
                //        tempMonth = anotherMonth;
                //    }

                //    var MonthStartDate = tempMonth;
                //    var MonthEndDate = MonthStartDate.AddMonths(1);

                //    var data = await collection.AsQueryable().Where(x => x.CreationTime >= MonthStartDate && x.CreationTime < MonthEndDate).CountAsync();

                //    monthCount = monthCount + data;
                //    tempMonth = MonthEndDate;

                //    Console.WriteLine(string.Format("Month:{0} {1}", MonthStartDate, MonthEndDate));
                //    Console.WriteLine(data);
                //}


                //Console.WriteLine("**************");

                //Console.WriteLine(monthCount);

                //var totalCount = await collection.AsQueryable().Where(x => x.CreationTime <= currentDate && x.CreationTime >= anotherMonth).CountAsync();
                //Console.WriteLine(totalCount);
                //Console.WriteLine("**************");

                // End Weeks - Month records    **************************************





                // All records

                //int start = 2018;
                //int end = DateTime.UtcNow.Year;
                //int yearsGap = end-start;
                //DateTime currentYearDate = DateTime.Today;
                //DateTime endYear= currentYearDate.AddYears(-yearsGap);

                //int monthCount = 0;
                //var tempYear = endYear;
                //for (int i = 1; i <= yearsGap; i++)
                //{
                //    if (i == 1)
                //    {
                //        tempYear = endYear;
                //    }

                //    var YearStartDate = tempYear;
                //    var YearEndDate = YearStartDate.AddYears(1); 

                //    var data = await collection.AsQueryable().Where(x => x.CreationTime >= YearStartDate && x.CreationTime < YearEndDate).CountAsync();

                //    monthCount = monthCount + data;
                //    tempYear = YearEndDate;

                //    Console.WriteLine(string.Format("Year:{0} {1}", YearStartDate, YearEndDate));
                //    Console.WriteLine(data);
                //}


                // Console.WriteLine("**************");

                // Console.WriteLine(monthCount);

                // var totalCount = await collection.AsQueryable().Where(x => x.CreationTime <= currentYearDate && x.CreationTime >= endYear).CountAsync();
                // Console.WriteLine(totalCount);
                 Console.WriteLine("**************");

                // End Weeks - Month records    **************************************






                // DateTime startDate = ;
                // DateTime endDate = new DateTime(year + 1, 1, 1,12,0,0,0);



                // var response = await collection.AsQueryable().Where(x => x.CreationTime >= startDate && x.CreationTime <= endDate).CountAsync();






            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
