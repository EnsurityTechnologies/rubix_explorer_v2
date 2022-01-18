using MongoDB.Driver;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Enums;
using Rubix.API.Shared.Interfaces;
using Rubix.API.Shared.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Repositories
{
    public class RepositoryDashboard : BaseRepository<Dashboard>, IRepositoryDashboard
    {
        public RepositoryDashboard(
            IMongoClient mongoClient,
            IClientSessionHandle clientSessionHandle) : base(mongoClient, clientSessionHandle, "_dashboard")
        {

        }


        public async Task<Dashboard> FindByAsync(ActivityFilter filter, EntityType type)
        {
            return await Collection.FindSync(x => x.EntityType == type && x.ActivityFilter == filter).FirstOrDefaultAsync();
        }
    }

    public class RepositoryCardsDashboard : BaseRepository<CardsDashboard>, IRepositoryCardsDashboard
    {
        public RepositoryCardsDashboard(
            IMongoClient mongoClient,
            IClientSessionHandle clientSessionHandle) : base(mongoClient, clientSessionHandle, "_cards_dashboard")
        {

        }

        public async Task<CardsDashboard> FindByAsync()
        {
            return await Collection.AsQueryable().FirstOrDefaultAsync();
        }
    }

    
}
