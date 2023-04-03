using Rubix.API.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Entities
{
    public class RubixDataToken : BaseEntity
    {
        public string transaction_id { get; set; }
        public string commiter { get; set; }
        public string sender { get; set; }
        public string receiver { get; set; }
        public double token_time { get; set; } 
        public double amount { get; set; }
        public TransactionType transaction_type { get; set; }
        public virtual string rbt_transaction_id { get; set; }

        public string quorum_list { get; set; }
        public string datatokens { get; set; }
    }
}
