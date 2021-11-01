using MongoDB.Bson.Serialization.Attributes;
using Rubix.API.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Entities
{
    public class RubixToken : BaseEntity
    {
        public RubixToken(string token_id, string bank_id, int denomination, string user_did,string level) =>
            (Token_id, Bank_id, Denomination, User_did, Level) = (token_id, bank_id, denomination, user_did,level);


        [BsonElement("token_id")]
        public virtual string Token_id { get; set; }

        [BsonElement("bank_id")]
        public virtual string Bank_id { get; set; }
        [BsonElement("denomination")]
        public virtual int Denomination { get; set; }

        [BsonElement("user_did")]
        public virtual string User_did { get; set; }

        [BsonElement("level")]
        public virtual string Level { get; set; } 
    }
}
