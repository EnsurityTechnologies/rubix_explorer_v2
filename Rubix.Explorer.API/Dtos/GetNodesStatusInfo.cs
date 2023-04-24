using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.Explorer.API.Dtos
{
    public class GetNodesStatusInfo
    {
        public long Total { get; set;}

        public long Online { get; set;}

        public long Offline { get; set;}
    }
}
