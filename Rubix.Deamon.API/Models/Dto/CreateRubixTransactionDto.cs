using Rubix.API.Shared.Entities;
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
        public double amount { get; set; }
        public TransactionType transaction_type { get; set; }
        public string nftToken { get; set; }
        public string nftBuyer { get; set; }
        public string nftSeller { get; set; }
        public string nftCreatorInput { get; set; }
        public virtual long totalSupply { get; set; }
        public virtual long editionNumber { get; set; }
        public virtual string rbt_transaction_id { get; set; }
        public virtual string userHash { get; set; }

        public virtual string block_hash { get; set;}
    }
}
