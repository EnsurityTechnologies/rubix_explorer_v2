using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Interfaces;
using Rubix.API.Shared.Repositories.Base;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using Rubix.API.Shared.Dto;
using DnsClient;
using MongoDB.Driver.Core.Misc;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;
using System.Numerics;
using MongoDB.Bson;

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
            await Collection.Indexes.CreateOneAsync(new CreateIndexModel<RubixUser>(Builders<RubixUser>.IndexKeys.Descending(d => d.User_did), new CreateIndexOptions { Unique = true }));
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
        public async Task<RubixUser> GetNodeByPeerIdAsync(string peerId)
        {
            var filter = Builders<RubixUser>.Filter.Eq(f => f.Peerid, peerId);
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

        public async Task<long> GetTotalNodeCountByStatus(bool iSOnline)
        {
            var filter = Builders<RubixUser>.Filter.Eq(f => f.IsOnline, iSOnline);
            return await Collection.Find(filter).CountAsync();
        }

        public async Task<List<UserBalanceInfo>> GetTopBalancesUserDids(int pageNumber,int pageSize)
        {
            var usersList = await Collection.AsQueryable().ToListAsync();

            var query = usersList.OrderByDescending(x => x.Balance);
            var pagedQuery = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var userBalanceInfoList = pagedQuery.Select(doc => new UserBalanceInfo
            {
                User_Did = doc.User_did,
                Balance = doc.Balance
            }).ToList();

            return userBalanceInfoList;
        }
    }
}

