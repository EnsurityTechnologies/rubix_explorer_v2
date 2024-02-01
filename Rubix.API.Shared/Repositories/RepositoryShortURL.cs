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
    public class RepositoryShortURL : BaseRepository<ShortUrl>, IRepositoryShortURL
    {
        public RepositoryShortURL(
            IMongoClient mongoClient,
            IClientSessionHandle clientSessionHandle) : base(mongoClient, clientSessionHandle, "_shortUrls")
        {

        }
        public async Task UpsertAsync(ShortUrl shortUrl)
        {
            var filter = Builders<ShortUrl>.Filter.Eq(s => s.Code, shortUrl.Code);

            // Use ReplaceOne with upsert true to insert or update
            var result = await this.Collection.ReplaceOneAsync(filter, shortUrl, new ReplaceOptions { IsUpsert = true });

            // You can check result.ModifiedCount to determine if it was an update (ModifiedCount > 0) or insert (ModifiedCount == 0)
        }

        public async Task<ShortUrl> FindByShortCodeAsync(string shortCode)
        {
            var filter = Builders<ShortUrl>.Filter.Eq(s => s.Code, shortCode);
            return await this.Collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
