using Rubix.API.Shared.Entities.Base;
using Rubix.API.Shared.Interfaces.Base;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Rubix.API.Shared.Common;
using Rubix.API.Shared.Enums;

namespace Rubix.API.Shared.Repositories.Base
{
    public class BaseRepository<T> : IRepositoryBase<T> where T : BaseEntity
    {
        private const string DATABASE = "rubixDb";
        private readonly IMongoClient _mongoClient;
        private readonly IClientSessionHandle _clientSessionHandle;
        private readonly string _collection;

        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public BaseRepository(IMongoClient mongoClient, IClientSessionHandle clientSessionHandle, string collection)
        {
            (_mongoClient, _clientSessionHandle, _collection) = (mongoClient, clientSessionHandle, collection);

            if (!_mongoClient.GetDatabase(DATABASE).ListCollectionNames().ToList().Contains(collection))
                _mongoClient.GetDatabase(DATABASE).CreateCollection(collection);
        }

        protected virtual IMongoCollection<T> Collection =>
            _mongoClient.GetDatabase(DATABASE).GetCollection<T>(_collection);

        public virtual async Task InsertAsync(T obj)
        {
            obj.CreationTime = DateTime.UtcNow;
            await Collection.InsertOneAsync(_clientSessionHandle, obj);
        }
            

        public virtual async Task InsertManyAsync(List<T> obj) =>
           await Collection.InsertManyAsync(_clientSessionHandle, obj);

        public virtual async Task UpdateAsync(T obj)
        {
            Expression<Func<T, string>> func = f => f.Id;
            var value = (string)obj.GetType().GetProperty(func.Body.ToString().Split(".")[1]).GetValue(obj, null);
            var filter = Builders<T>.Filter.Eq(func, value);

            if (obj != null)
            {
                obj.LastModificationTime = DateTime.UtcNow;
                await Collection.ReplaceOneAsync(_clientSessionHandle, filter, obj);
            }
        }

        public virtual async Task DeleteAsync(string id) =>
            await Collection.DeleteOneAsync(_clientSessionHandle, f => f.Id == id);


        public virtual async Task<PageResultDto<T>> GetPagerResultAsync(int page, int pageSize)
        {
            var filter = Builders<T>.Filter.Empty;
            var count = await Collection.Find(filter).CountAsync();
            var list = await Collection.Find(filter).SortByDescending(x => x.CreationTime).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

            return new PageResultDto<T>
            {
                Count = (int)count / pageSize,
                Size = pageSize,
                Page = page,
                Items = list
            };
        }

        public virtual async Task<long> GetCountAsync()
        {
            return  Collection.AsQueryable().Count();
        }

        public virtual async Task<IQueryable<T>> GetAllAsync()
        {
            return Collection.AsQueryable();
        }

        public virtual async Task<long> GetCountByRange(DateTime start,DateTime end)
        {
            var query = Collection.AsQueryable().Where(x => x.CreationTime.Value >= start && x.CreationTime.Value <= end).Count();    
            return query;
        }


        public virtual async Task<List<Resultdto>> GetAllTodayRecords()
        {

            List<Resultdto> resultdtos = new List<Resultdto>();
            var strathour = DateTime.UtcNow.AddHours(5.30).Date;
            int hour = 24;
            for (int i = 0; i <= hour; i++)
            {
                var hourStart = strathour.AddHours(i);
                var hourEnd = hourStart.AddMinutes(60).AddSeconds(-1);


                var totalCount = Collection.AsQueryable().Where(x => x.CreationTime >= hourStart && x.CreationTime <= hourEnd).Count();

                resultdtos.Add(new Resultdto()
                {
                    Key = hourStart.ToString("HH tt"),
                    Value = totalCount
                });
            }
            return resultdtos;
        }


        public virtual async Task<List<Resultdto>> GetAllByFilterAsync(ActivityFilter input)
        {
            switch (input)
            {
                case ActivityFilter.Today:
                    {
                        var todayNow = DateTime.UtcNow.AddHours(5.30);
                        var today = DateTime.UtcNow.Date;
                        var filterBuilder = Builders<T>.Filter;
                        var filter = filterBuilder.Gte(x => x.CreationTime, today) & filterBuilder.Lte(x => x.CreationTime, todayNow);

                        var query = Collection.Find(filter).ToEnumerable().Select(x => x.CreationTime).OrderBy(x => x.Value.Hour)
                                        .GroupBy(row => new
                                        {
                                            Hour = row.Value.ToString("hh tt")
                                        })
                                        .Select(grp => new Resultdto
                                        {
                                            Key = grp.Key.Hour,
                                            Value = grp.Count()
                                        });
                        return query.ToList();
                    }
                case ActivityFilter.Weekly:
                    {
                        var today = DateTime.UtcNow.Date;
                        var weekDay = today.AddDays(-7);
                        var filterBuilder = Builders<T>.Filter;
                        var filter = filterBuilder.Gte(x => x.CreationTime, weekDay) & filterBuilder.Lte(x => x.CreationTime, today);

                        var query = Collection.Find(filter).ToEnumerable().Select(x => x.CreationTime).OrderBy(x => x.Value.Day)
                                                    .GroupBy(row => new
                                                    {
                                                        Day = row.Value.ToString("dd/MMM/yyyy")
                                                    })
                                                    .Select(grp => new Resultdto
                                                    {
                                                        Key = grp.Key.Day,
                                                        Value = grp.Count()
                                                    });
                        return query.ToList();
                    }
                case ActivityFilter.Monthly:
                    {
                        var today = DateTime.UtcNow.Date;
                        var monthDay = today.AddMonths(-1);
                        var filterBuilder = Builders<T>.Filter;
                        var filter = filterBuilder.Gte(x => x.CreationTime, monthDay) & filterBuilder.Lte(x => x.CreationTime, today);

                        var query = Collection.Find(filter).ToEnumerable().Select(x => x.CreationTime).OrderBy(x => x.Value.Day).GroupBy(i => new 
                        {
                            MontlyData = i.Value.StartOfWeek(DateTime.UtcNow.DayOfWeek)
                        }).Select(grp => new Resultdto
                                {
                                    Key = grp.Key.MontlyData.ToString(),
                                    Value = grp.Count()
                                });

                        return query.ToList();
                    }
                case ActivityFilter.Quarterly:
                    {
                        var today = DateTime.UtcNow.Date;
                        var monthDay = today.AddMonths(-3);
                        var filterBuilder = Builders<T>.Filter;
                        var filter = filterBuilder.Gte(x => x.CreationTime, monthDay) & filterBuilder.Lte(x => x.CreationTime, today);

                        var query = Collection.Find(filter).ToEnumerable().Select(x => x.CreationTime).OrderBy(x => x.Value.Month)
                                                    .GroupBy(row => new
                                                    {
                                                        Month = row.Value.ToString("MMM")
                                                    })
                                                    .Select(grp => new Resultdto
                                                    {
                                                        Key = grp.Key.Month,
                                                        Value = grp.Count()
                                                    });
                        return query.ToList();


                    }
                case ActivityFilter.HalfYearly:
                    {
                        var today = DateTime.UtcNow.Date;
                        var monthDay = today.AddMonths(-6);
                        var filterBuilder = Builders<T>.Filter;
                        var filter = filterBuilder.Gte(x => x.CreationTime, monthDay) & filterBuilder.Lte(x => x.CreationTime, today);
                        var query = Collection.Find(filter).ToEnumerable().Select(x => x.CreationTime).OrderBy(x => x.Value.Month)
                                                  .GroupBy(row => new
                                                  {
                                                      Month = row.Value.ToString("MMM")
                                                  })
                                                  .Select(grp => new Resultdto
                                                  {
                                                      Key = grp.Key.Month,
                                                      Value = grp.Count()
                                                  });
                        return query.ToList();
                    }
                case ActivityFilter.Yearly:
                    {
                        var today = DateTime.UtcNow.Date;
                        var yearly = today.AddYears(-1);
                        var filterBuilder = Builders<T>.Filter;
                        var filter = filterBuilder.Gte(x => x.CreationTime, yearly) & filterBuilder.Lte(x => x.CreationTime, today);
                        var query = Collection.Find(filter).ToEnumerable().Select(x => x.CreationTime).OrderBy(x => x.Value.Month)
                                                  .GroupBy(row => new
                                                  {
                                                      Month = row.Value.ToString("MMM")
                                                  })
                                                  .Select(grp => new Resultdto
                                                  {
                                                      Key = grp.Key.Month,
                                                      Value = grp.Count()
                                                  });
                        return query.ToList();
                    }
                case ActivityFilter.All:
                    {
                        var filter = Builders<T>.Filter.Empty;
                        var query = Collection.Find(filter).ToEnumerable().Select(x => x.CreationTime).OrderBy(x => x.Value.Year)
                                                  .GroupBy(row => new
                                                  {
                                                      Year = row.Value.ToString("yyyy")
                                                  })
                                                  .Select(grp => new Resultdto
                                                  {
                                                      Key = grp.Key.Year,
                                                      Value = grp.Count()
                                                  });
                        return query.ToList();
                    }
                default:
                    return null;
            }

        }


        public virtual async Task<long> GetCountByFilterAsync(ActivityFilter input) 
        {
            switch (input)
            {
                case ActivityFilter.Today:
                    {

                        var startDay = DateTime.UtcNow.Date;
                        var endDay = startDay.AddHours(24);
                        var dayCounts = Collection.AsQueryable().Where(x => x.CreationTime >= startDay && x.CreationTime <= endDay).Count();
                        return dayCounts;
                    }
                case ActivityFilter.Weekly:
                    {
                        var weekStartDate = DateTime.Today.AddDays(-6).ToString("dd/MM/yyyy hh:mm:ss tt");
                        var weekendDate = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy hh:mm:ss tt");
                        var dayCounts = Collection.AsQueryable().Where(x => x.CreationTime >= Convert.ToDateTime(weekStartDate) && x.CreationTime <= Convert.ToDateTime(weekendDate)).Count();
                        return dayCounts;
                    }
                case ActivityFilter.Monthly:
                    {
                        var today = DateTime.UtcNow.Date;
                        var monthDay = today.AddMonths(-1);
                        var dayCounts = Collection.AsQueryable().Where(x => x.CreationTime <= today && x.CreationTime >= monthDay).Count();
                        return dayCounts;
                    }
                case ActivityFilter.Quarterly:
                    {
                        var today = DateTime.UtcNow.Date;
                        var monthDay = today.AddMonths(-3);
                        var dayCounts = Collection.AsQueryable().Where(x => x.CreationTime <= today && x.CreationTime >= monthDay).Count();
                        return dayCounts;
                    }
                case ActivityFilter.HalfYearly:
                    {
                        var today = DateTime.UtcNow.Date;
                        var monthDay = today.AddMonths(-6);
                        var dayCounts = Collection.AsQueryable().Where(x => x.CreationTime <= today && x.CreationTime >= monthDay).Count();
                        return dayCounts;
                    }
                case ActivityFilter.Yearly:
                    {
                        var today = DateTime.UtcNow.Date;
                        var yearly = today.AddYears(-1);
                        var dayCounts = Collection.AsQueryable().Where(x => x.CreationTime <= today && x.CreationTime >= yearly).Count();
                        return dayCounts;
                    }
                case ActivityFilter.All:
                    {
                        return  Collection.AsQueryable().Count();
                    }
                default:
                    return 0;
            }

        }
    }
}
