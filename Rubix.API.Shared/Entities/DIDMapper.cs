using MongoDB.Bson.Serialization.Attributes;
using Rubix.API.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Entities
{
    public class DIDMapper: BaseEntity
    {
        public DIDMapper(string newDID, string oldDID,string peerId,DateTime createdOn) =>
            (NewDID, OldDID,PeerID, CreatedOn) = (newDID, oldDID,peerId,createdOn);

        [BsonElement("old_did")]
        public virtual string OldDID { get; set; }

        [BsonElement("new_did")]
        public virtual string NewDID { get; set; }

        [BsonElement("peer_id")]
        public virtual string PeerID { get; set; } 

        [BsonElement("created_on")]
        public DateTime CreatedOn { get; set; }
    }
}
