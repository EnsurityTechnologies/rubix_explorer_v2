using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Rubix.API.Shared;
using Rubix.API.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loadertest
{
    class Program
    {

        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        private  static string CalculateDifference(DateTime fromData, DateTime toData)
        {
            TimeSpan ts = toData - fromData;
            string diff = string.Format("{0} hours, {1} minutes", ts.Hours, ts.Minutes);

            return diff;
        }
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




                //var startDay = DateTime.Today.Date;
                //var endDay = startDay.AddHours(24);
                //var dayCounts =await collection.AsQueryable().Where(x => x.CreationTime >= startDay && x.CreationTime <= endDay).CountAsync();

                //Console.WriteLine("today count");
                //Console.WriteLine(dayCounts);



                // 7 Days - Week   


                //long total = 0;


                //var todayNow = DateTime.Now;
                //var strathour = DateTime.Today.Date;

                //var total = 0;
                //int hour = 24;
                //for (int i = 0; i <= hour; i++)
                //{
                //    Console.WriteLine("***********");
                //    var hourStart = strathour.AddHours(i);
                //    var hourEnd = hourStart.AddMinutes(60);
                //    Console.WriteLine(hourStart.ToString("HH tt"));
                //    Console.WriteLine(hourEnd.ToString("HH tt"));

                //    var totalCount = await collection.AsQueryable().Where(x => x.CreationTime >= hourStart && x.CreationTime <= hourEnd).CountAsync();

                //    total = total + totalCount;
                //    Console.WriteLine(totalCount);

                //    Console.WriteLine("***********");
                //}

                //Console.WriteLine(total);
                // var test = CalculateDifference(strathour, Convert.ToDateTime(endhour));

                //                Console.WriteLine(test);


                //var test = DateTime.Today.AddDays(-6).ToString("dd/MM/yyyy hh:mm:ss tt");
                //var total = 0;
                //Console.WriteLine("**************");
                //for (int i = 1; i <= 7; i++)
                //{

                //    var end = Convert.ToDateTime(test).AddDays(i);

                //    var start = Convert.ToDateTime(end).AddHours(-24).ToString("dd/MM/yyyy hh:mm:ss tt");

                //    var vuu = Convert.ToDateTime(start);

                //    //Console.WriteLine("**************");
                //    Console.WriteLine(start);
                //    Console.WriteLine(end);
                //    //Console.WriteLine("**************");
                //    //var date = test.ToString("dd/MM/yyyy hh:mm:ss");
                //    //var nextDate = Convert.ToDateTime(date).AddDays(1);
                //    var dayCount = await collection.AsQueryable().Where(x => x.CreationTime.Value >= vuu && x.CreationTime.Value <= end).CountAsync();

                //    // Console.WriteLine(start);

                //    Console.WriteLine(dayCount);
                //    total = total + dayCount;
                //}

                //Console.WriteLine("**************");

                //Console.WriteLine("******Start********");
                //var weekStartDate = DateTime.Today.ToString("dd/MM/yyyy hh:mm:ss tt");
                //var weekendDate = DateTime.Today.AddDays(1).AddMinutes(-1).ToString("dd/MM/yyyy hh:mm:ss tt");

                //var dayCounts = await collection.AsQueryable().Where(x => x.CreationTime >= Convert.ToDateTime(weekStartDate) && x.CreationTime <= Convert.ToDateTime(weekendDate)).ToListAsync();
                //int count = 1;
                //foreach (var item in dayCounts)
                //{
                //    Console.WriteLine(string.Format("{0}. Date: {1} : TokenId:{2}", count, item.CreationTime,item.Token_id));
                //    count++;
                //}

                //Console.WriteLine(total);
                //Console.WriteLine("******end********");








                // Start Weeks - Month records    **************************************

                DateTime currentDate = DateTime.Today;//.AddDays(-14);
                DateTime anotherDate = currentDate.AddMonths(-2);
                DayOfWeek weekName = anotherDate.DayOfWeek;
                int totalWeeksPerMonth = currentDate.WeekdayCount(anotherDate, weekName);
                Console.WriteLine();

                int monthCount = 0;
                var tempDate = anotherDate;
                for (int i = 1; i <= totalWeeksPerMonth; i++)
                {
                    Console.WriteLine("Week:" + i);


                    if (i == 1)
                    {
                        tempDate = anotherDate;
                    }

                    var WeekStartDate = tempDate;
                    var WeekEndDate = WeekStartDate.AddDays(7);

                    var data = await collection.AsQueryable().Where(x => x.CreationTime >= WeekStartDate && x.CreationTime < WeekEndDate).CountAsync();
                    Console.WriteLine(data);

                    monthCount = monthCount + data;
                    tempDate = WeekEndDate;
                }

              //  Console.WriteLine(monthCount);

               // var totalCount = await collection.AsQueryable().Where(x => x.CreationTime <= currentDate && x.CreationTime >= anotherDate).CountAsync();
               // Console.WriteLine(totalCount);
               /// Console.WriteLine("**************");

                // End Weeks - Month records    **************************************









                // Start 3 Months - Qauterly records    **************************************

                //int totalMonthPerQuarter = 12;
                //DateTime currentDate = DateTime.Today;

                //var startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                //var endDate = startDate.AddMonths(1).AddDays(-1);


                //var latestDate = startDate.AddDays(DateTime.Now.Day);

                //Console.WriteLine("**************");
                //Console.WriteLine(latestDate.Month);

                //var tmpData = await collection.AsQueryable().Where(x => x.CreationTime >= startDate && x.CreationTime < latestDate).CountAsync();
                //Console.WriteLine(tmpData);
                //Console.WriteLine("**************");

                //for (int i = 1; i < totalMonthPerQuarter; i++)
                //{


                //        var tempstart = startDate.AddMonths(-i);
                //        var tempend = endDate.AddMonths(-i);

                //        Console.WriteLine(tempstart.Month);

                //        var data = await collection.AsQueryable().Where(x => x.CreationTime >= tempstart && x.CreationTime < tempend).CountAsync();
                //        Console.WriteLine(data);

                //        Console.WriteLine("**************");
                //}





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
