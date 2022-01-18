using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Rubix.API.Shared.Common;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Interfaces;
using Rubix.API.Shared.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Repositories
{
    public class RepositoryRubixTokenTransaction : BaseRepository<RubixTokenTransaction>, IRepositoryRubixTokenTransaction
    {
        public RepositoryRubixTokenTransaction(
            IMongoClient mongoClient,
            IClientSessionHandle clientSessionHandle) : base(mongoClient, clientSessionHandle, "_token_transactions")
        {

        }

        public async Task<RubixTokenTransaction> FindByTransIdAsync(string transId)
        {
            return  Collection.AsQueryable().Where(x => x.Transaction_id == transId).FirstOrDefault();
        }
        public async Task<PageResultDto<RubixTokenTransaction>> FindByTransByTokenIdAsync(string token_id,int pageSize,int page)
        {

            var filter = Builders<RubixTokenTransaction>.Filter.Empty;
            var count = await Collection.Find(filter).CountAsync();
            var list = await Collection.Find(filter).SortByDescending(x => x.CreationTime).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

            return new PageResultDto<RubixTokenTransaction>
            {
                Count = (int)count / pageSize,
                Size = pageSize,
                Page = page,
                Items = list
            };
        }
    }
}
