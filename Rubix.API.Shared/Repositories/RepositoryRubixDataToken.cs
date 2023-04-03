using MongoDB.Driver;
using Rubix.API.Shared.Common;
using Rubix.API.Shared.Dto;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Interfaces;
using Rubix.API.Shared.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        public virtual async Task<PageResultDto<DatatokenDto>> GetPagedResultAsync(int page, int pageSize)
        {

            var filter = Builders<RubixDataToken>.Filter.Empty;
            var count = await Collection.Find(filter).CountAsync();
            var list = await Collection.Find(filter).SortByDescending(x => x.CreationTime).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

            List<DatatokenDto> targetList = new List<DatatokenDto>();

            targetList.AddRange(list.Select(item => new DatatokenDto()
            {
                amount = item.amount,
                token_time = Math.Round((item.token_time) / 1000, 3),
                transaction_id = item.transaction_id,
                commiter = item.commiter,
                transaction_fee = 0,
                creation_time = item.CreationTime,
                volume = GetDataTokensCount(item.datatokens)


            })) ;
            return new PageResultDto<DatatokenDto>
            {
                Count = (int)count / pageSize,
                Size = pageSize,
                Page = page,
                Items = targetList
            };
        }

        private double GetDataTokensCount(string datatokens )
        {
            var datatokenlist = JsonSerializer.Deserialize<Dictionary<string, object>>( datatokens );
            return (double)datatokenlist.Count;
        }

        public async Task<RubixDataToken> FindByTransIdAsync(string transId)
        {
            return Collection.AsQueryable().Where(x => x.transaction_id == transId).FirstOrDefault();
        }


    }
}
