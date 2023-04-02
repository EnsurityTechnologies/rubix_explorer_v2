using Rubix.API.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Dto
{
    public class DatatokenDto
    {
        public string transaction_id { get; set; }
        public string commiter { get; set; }
        public double time { get; set; }
        public double amount { get; set; }
        public double token_time { get; set; }
        public int transaction_fee { get; set; }
        public DateTime? creation_time { get; set; }
        public TransactionType transaction_type { get; set; }
        public Dictionary<string, float> quorum_list { get; set; }
        public Dictionary<string, string> datatokens { get; set; }
    }
}
