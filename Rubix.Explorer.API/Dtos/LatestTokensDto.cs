using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.Explorer.API.Dtos
{
    public class TokenDtoPagedResultDto
    {
        public int TotalCount { get; set; }
        public List<LatestTokensDto> Items { get; set; }
    }
    public class LatestTokensDto
    {
        public string token_id { get; set; }
        public string user_did { get; set; }
    }
}
