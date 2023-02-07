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
        public DIDMapper(string newDID, string oldDID,DateTime createdOn) =>
            (NewDID, OldDID, CreatedOn) = (newDID, oldDID, createdOn);

        [BsonElement("old_did")]
        public virtual string OldDID { get; set; }

        [BsonElement("new_did")]
        public virtual string NewDID { get; set; }

        [BsonElement("created_on")]
        public DateTime CreatedOn { get; set; }
    }
}
