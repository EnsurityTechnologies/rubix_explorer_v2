using Rubix.API.Shared.Common;
using Rubix.API.Shared.Dto;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Interfaces
{
    public interface IRepositoryRubixDataToken : IRepositoryBase<RubixDataToken>
    {
        Task<PageResultDto<DatatokenDto>> GetPagedResultAsync(int page, int pageSize);
        Task<RubixDataToken> FindByTransIdAsync(string transId);
    }
}
