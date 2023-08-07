using Rubix.API.Shared.Common;
using Rubix.API.Shared.Dto;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Interfaces
{
    public interface IRepositoryRubixTransaction : IRepositoryBase<RubixTransaction>
    {
        Task<PageResultDto<TransactionDto>> GetPagedResultAsync(int page, int pageSize);

        Task<RubixTransaction> FindByTransIdAsync(string transId);

        Task<PageResultDto<TransactionDto>> GetPagedResultByDIDAsync(string did, int page, int pageSize);

        Task<PageResultDto<TopWalletsDto>> GetTopWalletsAsync();

        Task<List<string>> GetSenderTransactionListByDIDAsync(string did);

        Task<List<string>> GetReciverTransactionListByDIDAsync(string did);

    }
}
