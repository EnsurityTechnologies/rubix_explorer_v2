using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Interfaces
{
    public interface IDIDMapperRepository : IRepositoryBase<DIDMapper>
    {
        Task<DIDMapper> GetOldDIDInfo(string newdid);

        Task<DIDMapper> GetNewDIDInfo(string oldDID);
    }
}
