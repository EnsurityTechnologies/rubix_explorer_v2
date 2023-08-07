using MongoDB.Driver;
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

        public virtual async Task<bool> IsMinedToken(string tokenHash)
        {
            var tokenExit= await Collection.FindAsync<RubixToken>(f => f.Token_id == tokenHash);
            var tokenInfo = await tokenExit.FirstOrDefaultAsync();
            if (tokenInfo != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual async Task<IEnumerable<Resultdto>> GetLayerBasedMinedTokensCount()
        {
            var filterBuilder = Builders<RubixToken>.Filter.Empty;
        
            var query = Collection.Find(filterBuilder).ToEnumerable().Select(x => x.Level)
                            .GroupBy(row => new
                            {
                                Level = row,
                                Count=row.Count()
                            })
                            .Select(grp => new Resultdto
                            {
                                Key = grp.Key.ToString(),
                                Value = grp.Count()
                            }).ToList();
            return query;
        }

        public virtual async Task<long> GetCountByUserDIDAsync(string user_did)
        {
            return Collection.AsQueryable().Where(x => x.User_did == user_did).Count();
        }
    }
}
