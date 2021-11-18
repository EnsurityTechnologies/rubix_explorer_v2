using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Dto
{
    public class CardsDto
    {
        public long TransCount { get; set; }

        public long TokensCount { get; set; }

        public long UsersCount { get; set; }

        public long CirculatingSupply { get; set; }
    }
}
