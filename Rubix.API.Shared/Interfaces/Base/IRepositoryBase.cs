using Rubix.API.Shared.Common;
using Rubix.API.Shared.Entities.Base;
using Rubix.API.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Interfaces.Base
{
    public interface IRepositoryBase<T> where T : BaseEntity
    {

        Task<PageResultDto<T>> GetPagerResultAsync(int page, int pageSize);

        Task InsertAsync(T obj);

        Task InsertManyAsync(List<T> obj);

        Task UpdateAsync(T obj);

        Task DeleteAsync(string id);

        Task<long> GetCountAsync();


        Task<IQueryable<T>> GetAllAsync();


        Task<long> GetCountByRange(DateTime start, DateTime end);

    }
}
