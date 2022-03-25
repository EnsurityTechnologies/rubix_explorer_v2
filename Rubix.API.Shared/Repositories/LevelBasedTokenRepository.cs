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
    public class LevelBasedTokenRepository : BaseRepository<LevelBasedTokens>, ILevelBasedTokenRepository
    {
        public LevelBasedTokenRepository(IMongoClient mongoClient,
            IClientSessionHandle clientSessionHandle) : base(mongoClient, clientSessionHandle, "_levelBasedToken")
        {

        }
    }
}