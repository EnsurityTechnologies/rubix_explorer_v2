using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Rubix.API.Shared.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Rubix.Explorer.API
{
    public class ConfigureMongoDbIndexesService : IHostedService
    {
        private readonly IMongoClient _client;
     
        public ConfigureMongoDbIndexesService(IMongoClient client)
            => (_client) = (client);

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var database = _client.GetDatabase("rubixDb");


            //Transactions
            var _transCollection = database.GetCollection<RubixTransaction>("_transactions");
            var index_transCollectionKeysDefinition = Builders<RubixTransaction>.IndexKeys.Descending(x => x.CreationTime);
            await _transCollection.Indexes.CreateOneAsync(new CreateIndexModel<RubixTransaction>(index_transCollectionKeysDefinition), cancellationToken: cancellationToken);
            //Tokens

            var _tokensCollection = database.GetCollection<RubixToken>("_tokens");
            var index_tokensCollectionKeysDefinition = Builders<RubixToken>.IndexKeys.Descending(x => x.CreationTime);
            await _tokensCollection.Indexes.CreateOneAsync(new CreateIndexModel<RubixToken>(index_tokensCollectionKeysDefinition), cancellationToken: cancellationToken);

            //TokenTransaction

            var _tokenTransCollection = database.GetCollection<RubixTokenTransaction>("_token_transactions");
            var index_tokenTransCollectionKeysDefinition = Builders<RubixTokenTransaction>.IndexKeys.Descending(x => x.CreationTime);
            await _tokenTransCollection.Indexes.CreateOneAsync(new CreateIndexModel<RubixTokenTransaction>(index_tokenTransCollectionKeysDefinition), cancellationToken: cancellationToken);


            var index_tokenTransTokenIdCollectionKeysDefinition = Builders<RubixTokenTransaction>.IndexKeys.Descending(x => x.Token_id);
            await _tokenTransCollection.Indexes.CreateOneAsync(new CreateIndexModel<RubixTokenTransaction>(index_tokenTransTokenIdCollectionKeysDefinition), cancellationToken: cancellationToken);

            var index_tokenTransTrasnIdCollectionKeysDefinition = Builders<RubixTokenTransaction>.IndexKeys.Descending(x => x.Transaction_id);
            await _tokenTransCollection.Indexes.CreateOneAsync(new CreateIndexModel<RubixTokenTransaction>(index_tokenTransTrasnIdCollectionKeysDefinition), cancellationToken: cancellationToken);

        }


        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
