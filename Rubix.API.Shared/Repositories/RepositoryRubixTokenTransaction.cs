using MongoDB.Driver;
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
            return await Collection.FindSync(x => x.Transaction_id == transId).FirstOrDefaultAsync();
        }
    }
}
