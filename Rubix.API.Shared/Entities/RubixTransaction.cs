using MongoDB.Bson.Serialization.Attributes;
using Rubix.API.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Entities
{
    public class RubixTransaction : BaseEntity
    {
        public RubixTransaction(string transaction_id, string sender_did, string receiver_did, double token_time, int amount) =>
            (Transaction_id, Sender_did, Receiver_did, Token_time, Amount) = (transaction_id, sender_did, receiver_did, token_time, amount);


        [BsonElement("transaction_id")]
        public virtual string Transaction_id { get; set; }

        [BsonElement("sender_did")]
        public virtual string Sender_did { get; set; }

        [BsonElement("receiver_did")]
        public virtual string Receiver_did { get; set; }

        [BsonElement("token_time")]
        public virtual double Token_time { get; set; }

        [BsonElement("amount")]
        public virtual int Amount { get; set; } 
    }
}
