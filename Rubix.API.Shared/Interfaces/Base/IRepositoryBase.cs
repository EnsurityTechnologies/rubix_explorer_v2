using Rubix.API.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Interfaces.Base
{
    public interface IRepositoryBase<T> where T : BaseEntity
    {
        Task InsertAsync(T obj);

        Task InsertManyAsync(List<T> obj);

        Task UpdateAsync(T obj);

        Task DeleteAsync(string id);
    }
}
