using MongoDB.Driver;
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
    public class RepositoryRubixDataToken : BaseRepository<RubixDataToken>, IRepositoryRubixDataToken
    {
        public RepositoryRubixDataToken(
            IMongoClient mongoClient,
            IClientSessionHandle clientSessionHandle) : base(mongoClient, clientSessionHandle, "_datatokens")
        {

        }


        public override async Task InsertManyAsync(List<RubixDataToken> obj)
        {
            await Collection.Indexes.CreateOneAsync(new CreateIndexModel<RubixDataToken>(Builders<RubixDataToken>.IndexKeys.Descending(d => d.transaction_id), new CreateIndexOptions { Unique = true }));
            await base.InsertManyAsync(obj);
        }

    }
}
