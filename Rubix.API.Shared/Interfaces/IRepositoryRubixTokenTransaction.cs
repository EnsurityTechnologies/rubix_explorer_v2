using Rubix.API.Shared.Common;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Interfaces
{
    public interface IRepositoryRubixTokenTransaction : IRepositoryBase<RubixTokenTransaction>
    {
        Task<RubixTokenTransaction> FindByTransIdAsync(string transId);
        Task<PageResultDto<RubixTokenTransaction>> FindByTransByTokenIdAsync(string token_id, int pageSize, int page);
        Task<long> CountByTransIdAsync(string transId);
    }
}
