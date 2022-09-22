using MongoDB.Bson.Serialization.Attributes;
using Rubix.API.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Entities
{
    public class RubixTransactionQuorum : BaseEntity
    {
        public RubixTransactionQuorum(string transaction_id, string quorum_list) =>
            (Transaction_id, Quorum_List) = (transaction_id, quorum_list); 

        [BsonElement("transaction_id")]
        public virtual string Transaction_id { get; set; }

        [BsonElement("quorum_list")]
        public virtual string Quorum_List { get; set; } 
    }
}
