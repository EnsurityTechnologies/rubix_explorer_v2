using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.Deamon.API.Models.Dto
{
    public class CreateRubixTransactionDto
    {
        public string transaction_id { get; set; }
        public string sender_did { get; set; }
        public string receiver_did { get; set; }
        public double token_time { get; set; }
        public List<string> token_id { get; set; }
        public int amount { get; set; }
    }
}
