using Rubix.API.Shared.Entities.Base;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Entities
{
    public class RubixUser : BaseEntity
    {
        public RubixUser(string user_did, string peerid,string ipaddress,int balance) =>
            (User_did, Peerid,IPaddress,Balance) = (user_did, peerid, ipaddress, balance);


        [BsonElement("user_did")]
        public virtual string User_did { get; set; }

        [BsonElement("peerid")]
        public virtual string Peerid { get; set; }

        [BsonElement("ipaddress")]
        public virtual string IPaddress { get; set; }

        [BsonElement("balance")]
        public virtual int Balance { get; set; } 
    }
}
