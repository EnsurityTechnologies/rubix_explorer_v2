using Rubix.API.Shared.Common;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Interfaces
{
    public interface IRepositoryRubixToken : IRepositoryBase<RubixToken>
    {
        Task<bool> IsMinedToken(string tokenHash);

        Task<IEnumerable<Resultdto>> GetLayerBasedMinedTokensCount();
    }
}
