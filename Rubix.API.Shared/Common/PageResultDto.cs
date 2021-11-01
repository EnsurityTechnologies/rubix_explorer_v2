using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Common
{
    public class PageResultDto<T> 
    {
        public int Count { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
