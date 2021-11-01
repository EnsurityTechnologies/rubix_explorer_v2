using Rubix.API.Shared.Entities.Base;
using Rubix.API.Shared.Interfaces.Base;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Repositories.Base
{
    public class BaseRepository<T> : IRepositoryBase<T> where T : BaseEntity
    {
        private const string DATABASE = "rubixDb";
        private readonly IMongoClient _mongoClient;
        private readonly IClientSessionHandle _clientSessionHandle;
        private readonly string _collection;

        public BaseRepository(IMongoClient mongoClient, IClientSessionHandle clientSessionHandle, string collection)
        {
            (_mongoClient, _clientSessionHandle, _collection) = (mongoClient, clientSessionHandle, collection);

            if (!_mongoClient.GetDatabase(DATABASE).ListCollectionNames().ToList().Contains(collection))
                _mongoClient.GetDatabase(DATABASE).CreateCollection(collection);
        }

        protected virtual IMongoCollection<T> Collection =>
            _mongoClient.GetDatabase(DATABASE).GetCollection<T>(_collection);

        public virtual async Task InsertAsync(T obj)
        {
            obj.CreationTime = DateTime.UtcNow;
            await Collection.InsertOneAsync(_clientSessionHandle, obj);
        }
            

        public virtual async Task InsertManyAsync(List<T> obj) =>
           await Collection.InsertManyAsync(_clientSessionHandle, obj);

        public virtual async Task UpdateAsync(T obj)
        {
            Expression<Func<T, string>> func = f => f.Id;
            var value = (string)obj.GetType().GetProperty(func.Body.ToString().Split(".")[1]).GetValue(obj, null);
            var filter = Builders<T>.Filter.Eq(func, value);

            if (obj != null)
            {
                obj.LastModificationTime = DateTime.UtcNow;
                await Collection.ReplaceOneAsync(_clientSessionHandle, filter, obj);
            }
        }

        public virtual async Task DeleteAsync(string id) =>
            await Collection.DeleteOneAsync(_clientSessionHandle, f => f.Id == id);
    }
}
