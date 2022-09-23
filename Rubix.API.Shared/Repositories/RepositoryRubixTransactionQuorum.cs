﻿using MongoDB.Driver;
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
    public class RepositoryRubixTransactionQuorum : BaseRepository<RubixTransactionQuorum>, IRepositoryRubixTransactionQuorum
    {
        public RepositoryRubixTransactionQuorum(
            IMongoClient mongoClient,
            IClientSessionHandle clientSessionHandle) : base(mongoClient, clientSessionHandle, "_transaction_Quorums")
        {

        }

        public async Task<RubixTransactionQuorum> FindByTransIdAsync(string transId)
        {
            return Collection.AsQueryable().Where(x => x.Transaction_id == transId).FirstOrDefault();
        }
    }
}