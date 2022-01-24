using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Dto
{
    public class TransactionDto
    {
        public string transaction_id { get; set; }

        public string sender_did { get; set; }

        public string receiver_did { get; set; }

        public double token_time { get; set; }

        public double amount { get; set; }

        public int transaction_fee { get; set; }

        public double time { get; set; }
    }
}
