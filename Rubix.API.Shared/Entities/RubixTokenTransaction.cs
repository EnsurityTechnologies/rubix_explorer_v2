using MongoDB.Bson.Serialization.Attributes;
using Rubix.API.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Entities
{
    public class RubixTokenTransaction : BaseEntity
    {
        public RubixTokenTransaction(string transaction_id, string token_id) =>
            (Transaction_id, Token_id) = (transaction_id, token_id);

        [BsonElement("transaction_id")]
        public virtual string Transaction_id { get; set; }

        [BsonElement("token_id")]
        public virtual string Token_id { get; set; } 
    }
}
