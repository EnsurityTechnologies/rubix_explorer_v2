using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Interfaces
{
    public interface IRepositoryRubixUser : IRepositoryBase<RubixUser> 
    {
        Task<RubixUser> GetUserByUser_DIDAsync(string user_did);
        Task<RubixUser> GetUserAsync(string id);
        Task<IEnumerable<RubixUser>> GetUsersAsync();
    }
}
