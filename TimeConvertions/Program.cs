using Rubix.API.Shared;
using Rubix.API.Shared.Enums;
using System;

namespace TimeConvertions
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*********************************************");

            //Transaction , Tokens Live charts data updates.

            var values = EnumUtil.GetValues<ActivityFilter>();
            foreach (var activeity in values)
            {
                switch (activeity)
                {
                    case ActivityFilter.Today:
                        {


                           
                        }
                        break;
                    case ActivityFilter.Weekly:
                        {


                            //Transactions

                          
                            for (int i = 1; i <= 7; i++)
                            {
                                var date = DateTime.Now.ToUniversalTime().AddDays(-i).Date;
                                var nextDate = date.AddDays(1).Date;
                                Console.WriteLine(nextDate);
                            }
                           
                        }

                        break;
                    case ActivityFilter.Monthly:
                        {
                            DateTime currentDate = DateTime.Now.ToUniversalTime();
                            DateTime anotherDate = currentDate.AddMonths(-1);
                            DayOfWeek weekName = anotherDate.DayOfWeek;
                            int totalWeeksPerMonth = currentDate.WeekdayCount(anotherDate, weekName);

                            var tempDate = anotherDate;

                            //Transactions And Tokens

                          
                            for (int i = 1; i <= totalWeeksPerMonth; i++)
                            {
                                if (i == 1)
                                {
                                    tempDate = anotherDate;
                                }
                                var WeekStartDate = tempDate;
                                var WeekEndDate = WeekStartDate.AddDays(7);

                              

                                tempDate = WeekEndDate;
                            }
                        }
                        break;
                    case ActivityFilter.Quarterly:
                        {

                            int months = 3;
                            DateTime currentDate = DateTime.Now.ToUniversalTime();
                            DateTime anotherMonth = currentDate.AddMonths(-months);
                            var tempMonth = anotherMonth;

                            for (int i = 1; i <= months; i++)
                            {
                                if (i == 1)
                                {
                                    tempMonth = anotherMonth;
                                }
                                var MonthStartDate = tempMonth;
                                var MonthEndDate = MonthStartDate.AddMonths(1);

                             
                                tempMonth = MonthEndDate;
                            }



                          
                        }
                        break;
                    case ActivityFilter.HalfYearly:
                        {
                            int months = 6;
                            DateTime currentDate = DateTime.Now.ToUniversalTime();
                            DateTime anotherMonth = currentDate.AddMonths(-months);
                            var tempMonth = anotherMonth;


                           
                            for (int i = 1; i <= months; i++)
                            {
                                if (i == 1)
                                {
                                    tempMonth = anotherMonth;
                                }
                                var MonthStartDate = tempMonth;
                                var MonthEndDate = MonthStartDate.AddMonths(1);

                           
                                tempMonth = MonthEndDate;
                            }



                           
                        }
                        break;
                    case ActivityFilter.Yearly:
                        {
                            int months = 12;
                            DateTime currentDate = DateTime.Now.ToUniversalTime();
                            DateTime anotherMonth = currentDate.AddMonths(-months);
                            var tempMonth = anotherMonth;

                            for (int i = 1; i <= months; i++)
                            {
                                if (i == 1)
                                {
                                    tempMonth = anotherMonth;
                                }
                                var MonthStartDate = tempMonth;
                                var MonthEndDate = MonthStartDate.AddMonths(1);

                               
                                tempMonth = MonthEndDate;
                            }



                           
                        }
                        break;
                    case ActivityFilter.All:
                        {
                            int start = 2018;
                            int end = DateTime.UtcNow.Year;
                            int yearsGap = end - start;
                            DateTime currentYearDate = DateTime.Now.ToUniversalTime();
                            DateTime endYear = currentYearDate.AddYears(-yearsGap);

                            var tempYear = endYear;


                            for (int i = 1; i <= yearsGap; i++)
                            {
                                if (i == 1)
                                {
                                    tempYear = endYear;
                                }

                                var YearStartDate = tempYear;
                                var YearEndDate = YearStartDate.AddYears(1);

                               
                                tempYear = YearEndDate;
                            }
                        }
                        break;
                }

            }

            Console.WriteLine("Completed");
        }
    }
}
