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

            var queryable = Collection.AsQueryable().OrderByDescending(e => e.CreationTime).Where(x => x.Token_id==token_id);

            var count =await queryable.CountAsync();

            var list= queryable.Skip((page - 1) * pageSize).Take(pageSize);

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
