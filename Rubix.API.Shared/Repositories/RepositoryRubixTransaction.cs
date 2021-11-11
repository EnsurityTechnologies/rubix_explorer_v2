using MongoDB.Bson;
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
    public class RepositoryRubixTransaction : BaseRepository<RubixTransaction>, IRepositoryRubixTransaction
    {
        public RepositoryRubixTransaction(
            IMongoClient mongoClient,
            IClientSessionHandle clientSessionHandle) : base(mongoClient, clientSessionHandle, "_transactions")
        {

        }

        public override async Task InsertAsync(RubixTransaction obj)
        {
            await Collection.Indexes.CreateOneAsync(new CreateIndexModel<RubixTransaction>(Builders<RubixTransaction>.IndexKeys.Ascending(d => d.Transaction_id), new CreateIndexOptions { Unique = true }));
            await base.InsertAsync(obj);
        }
    }
}
