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
    public class DIDMapperRepository  : BaseRepository<DIDMapper>, IDIDMapperRepository
    {
        public DIDMapperRepository(IMongoClient mongoClient,
            IClientSessionHandle clientSessionHandle) : base(mongoClient, clientSessionHandle, "_didMapperCollection")
        {

        }

        public override async Task InsertAsync(DIDMapper obj)
        {
            await Collection.Indexes.CreateOneAsync(new CreateIndexModel<DIDMapper>(Builders<DIDMapper>.IndexKeys.Descending(d => d.OldDID), new CreateIndexOptions { Unique = true }));
            await base.InsertAsync(obj);
        }
    }
}
