using MongoDB.Bson;
using MongoDB.Driver;
using Rubix.API.Shared.Common;
using Rubix.API.Shared.Dto;
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


        public async Task<RubixTransaction> FindByTransIdAsync(string transId)
        {
            return  Collection.AsQueryable().Where(x => x.Transaction_id == transId).FirstOrDefault();
        }


        public override async Task InsertAsync(RubixTransaction obj)
        {
            await Collection.Indexes.CreateOneAsync(new CreateIndexModel<RubixTransaction>(Builders<RubixTransaction>.IndexKeys.Ascending(d => d.Transaction_id), new CreateIndexOptions { Unique = true }));
            await base.InsertAsync(obj);
        }

        public virtual async Task<PageResultDto<TransactionDto>> GetPagedResultAsync(int page, int pageSize)
        {
            // count facet, aggregation stage of count
            //var countFacet = AggregateFacet.Create("countFacet",
            //    PipelineDefinition<RubixTransaction, AggregateCountResult>.Create(new[]
            //    {
            //    PipelineStageDefinitionBuilder.Count<RubixTransaction>()
            //    }));

            //// data facet, we’ll use this to sort the data and do the skip and limiting of the results for the paging.
            //var dataFacet = AggregateFacet.Create("dataFacet",
            //    PipelineDefinition<RubixTransaction, RubixTransaction>.Create(new[]
            //    {
            //        PipelineStageDefinitionBuilder.Sort(Builders<RubixTransaction>.Sort.Descending(x => x.CreationTime)),
            //        PipelineStageDefinitionBuilder.Skip<RubixTransaction>((page - 1) * pageSize),
            //        PipelineStageDefinitionBuilder.Limit<RubixTransaction>(pageSize),
            //    }));

            var filter = Builders<RubixTransaction>.Filter.Empty;
            var count = await Collection.Find(filter).CountAsync();
            var list = await Collection.Find(filter).SortByDescending(e => e.CreationTime).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

            //var filter = Builders<RubixTransaction>.Filter.Empty;

            //var aggregation = await Collection.Aggregate()
            //    .Match(filter)
            //    .Facet(countFacet, dataFacet).Limit(pageSize)
            //    .ToListAsync();





            //var count = aggregation.First()
            //    .Facets.First(x => x.Name == "countFacet")
            //    .Output<AggregateCountResult>()
            //    ?.FirstOrDefault()
            //    ?.Count ?? 0;

            //var data = aggregation.First()
            //    .Facets.First(x => x.Name == "dataFacet")
            //    .Output<RubixTransaction>().ToList();

            List<TransactionDto> targetList = new List<TransactionDto>();

            targetList.AddRange(list.Select(item => new TransactionDto()
            {
                amount = item.Amount,
                token_time = Math.Round((item.Token_time / item.Amount) / 1000, 3),
                receiver_did = item.Receiver_did,
                transaction_id = item.Transaction_id,
                sender_did = item.Sender_did,
                time = item.Token_time,
                transaction_fee = 0
            }));
            return new PageResultDto<TransactionDto>
            {
                Count = (int)count / pageSize,
                Size = pageSize,
                Page = page,
                Items = targetList
            };
        }
    }
}
