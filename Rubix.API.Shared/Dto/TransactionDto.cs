using Rubix.API.Shared.Entities;
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

        public virtual TransactionType TransactionType { get; set; }

        public virtual string NftToken { get; set; }

        public virtual string NftBuyer { get; set; }

        public virtual string NftSeller { get; set; }

        public virtual string NftCreatorInput { get; set; }


        public virtual long TotalSupply { get; set; }

        public virtual long EditionNumber { get; set; }

        public virtual string RBTTransactionId { get; set; }

        public virtual string UserHash { get; set; }

        public virtual string BlockHash { get; set; }
    }
}
