using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.Explorer.API.Dtos
{
    public class RubixAnalyticsDto
    {
        public double? RubixPrice { get; set; }
        public long TransactionsCount { get; set; }
        public long TokensCount { get; set; }
        public long RubixUsersCount { get; set; }

        public long CurculatingSupplyCount { get; set;}
    }
}
