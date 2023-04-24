using MongoDB.Driver;
using Rubix.API.Shared.Common;
using Rubix.API.Shared.Dto;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Interfaces;
using Rubix.API.Shared.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Repositories
{
    public class RepositoryRubixNFTTransaction : BaseRepository<RubixNFTTransaction>, IRepositoryRubixNFTTransaction
    {
        public RepositoryRubixNFTTransaction(
            IMongoClient mongoClient,
            IClientSessionHandle clientSessionHandle) : base(mongoClient, clientSessionHandle, "_nft_transactions")
        {

        }


        public async Task<RubixNFTTransaction> FindByTransIdAsync(string transId)
        {
            return Collection.AsQueryable().Where(x => x.RBTTransactionId == transId).FirstOrDefault();
        }


        public override async Task InsertAsync(RubixNFTTransaction obj)
        {
            await Collection.Indexes.CreateOneAsync(new CreateIndexModel<RubixNFTTransaction>(Builders<RubixNFTTransaction>.IndexKeys.Descending(d => d.RBTTransactionId), new CreateIndexOptions { Unique = true }));
            await base.InsertAsync(obj);
        }

        public virtual async Task<PageResultDto<TransactionDto>> GetPagedResultAsync(int page, int pageSize)
        {

            var filter = Builders<RubixNFTTransaction>.Filter.Empty;
            var count = await Collection.Find(filter).CountAsync();
            var list = await Collection.Find(filter).SortByDescending(x => x.CreationTime).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

            List<TransactionDto> targetList = new List<TransactionDto>();

            targetList.AddRange(list.Select(item => new TransactionDto()
            {
                
            }));
            return new PageResultDto<TransactionDto>
            {
                Count = (int)count / pageSize,
                Size = pageSize,
                Page = page,
                Items = targetList
            };
        }


        public virtual async Task<PageResultDto<TransactionDto>> GetPagedResultByDIDAsync(string did, int page, int pageSize)
        {
            var filter = Builders<RubixNFTTransaction>.Filter.Eq(x => x.NftSeller, did) | Builders<RubixNFTTransaction>.Filter.Eq(x => x.NftBuyer, did);

            var count = await Collection.Find(filter).CountAsync();

            var list = await Collection.Find(filter).SortByDescending(x => x.CreationTime).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

            List<TransactionDto> targetList = new List<TransactionDto>();

            targetList.AddRange(list.Select(item => new TransactionDto()
            {
                transaction_fee = 0,
                CreationTime = item.CreationTime,
            }));
            return new PageResultDto<TransactionDto>
            {
                Count = (int)count / pageSize,
                Size = pageSize,
                Page = page,
                Items = targetList
            };
        }
    }
}
