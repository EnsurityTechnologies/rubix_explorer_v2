using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Interfaces;
using Rubix.API.Shared.Repositories.Base;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Repositories
{
    public class RepositoryRubixUser : BaseRepository<RubixUser>, IRepositoryRubixUser
    {
        public RepositoryRubixUser(
            IMongoClient mongoClient,
            IClientSessionHandle clientSessionHandle) : base(mongoClient, clientSessionHandle, "_users")
        {

        }


        public override async Task InsertAsync(RubixUser obj)
        {
            await Collection.Indexes.CreateOneAsync(new CreateIndexModel<RubixUser>(Builders<RubixUser>.IndexKeys.Ascending(d => d.User_did),new CreateIndexOptions { Unique = true }));
            await base.InsertAsync(obj);
        }


        public async Task<RubixUser> GetUserAsync(string id)
        {
            var filter = Builders<RubixUser>.Filter.Eq(f => f.Id, id); 
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<RubixUser> GetUserByUser_DIDAsync(string user_did) 
        {
            var filter = Builders<RubixUser>.Filter.Eq(f => f.User_did, user_did);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RubixUser>> GetUsersAsync()
        {
            //Collection.Find(filter).Project(p => p.Books).FirstOrDefaultAsync();

            //var start = new DateTime(2021, 10, 28);
            //var end = new DateTime(2021, 11, 1);

            //var filterBuilder = Builders<RubixUser>.Filter;
            //var filter = filterBuilder.Gte(x => x.CreationTime, start) & filterBuilder.Lte(x => x.CreationTime, end);

            //return await Collection.Find(filter).ToListAsync();


            return await Collection.AsQueryable().ToListAsync();
        } 
    }
}
