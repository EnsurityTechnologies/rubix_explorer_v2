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
    public class RepositoryRubixToken : BaseRepository<RubixToken>, IRepositoryRubixToken
    {
        public RepositoryRubixToken(
            IMongoClient mongoClient,
            IClientSessionHandle clientSessionHandle) : base(mongoClient, clientSessionHandle, "_tokens")
        {

        }


        public override async Task InsertManyAsync(List<RubixToken> obj)
        {
            await Collection.Indexes.CreateOneAsync(new CreateIndexModel<RubixToken>(Builders<RubixToken>.IndexKeys.Descending(d => d.Token_id), new CreateIndexOptions { Unique = true }));
            await base.InsertManyAsync(obj);
        }
    }
}
