﻿using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Enums;
using Rubix.API.Shared.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Interfaces
{
    public interface IRepositoryDashboard : IRepositoryBase<Dashboard>
    {
        Task<Dashboard> FindByAsync(ActivityFilter filter, EntityType type);  
    }

    public interface IRepositoryCardsDashboard : IRepositoryBase<CardsDashboard> 
    {
        Task<CardsDashboard> FindByAsync(); 
    }

    
}
