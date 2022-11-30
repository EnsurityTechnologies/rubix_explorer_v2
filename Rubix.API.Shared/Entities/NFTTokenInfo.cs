using MongoDB.Bson.Serialization.Attributes;
using Rubix.API.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Entities
{
    public class NFTTokenInfo : BaseEntity
    {
        public NFTTokenInfo(string type,string creatorId,string nftToken,string creatorPubKeyIpfsHash,long totalSupply,long edition,string url,string createdOn) =>
            (Type, CreatorId, NFTToken, CreatorPubKeyIpfsHash, TotalSupply, Edition, URL, CreatedOn) = (type,creatorId, nftToken, creatorPubKeyIpfsHash, totalSupply, edition,url, createdOn);


        [BsonElement("tokenType")]
        public virtual string Type { get; set; } 

        [BsonElement("creatorId")]
        public virtual string CreatorId { get; set; }
       
        [BsonElement("nftToken")]

        public virtual string NFTToken { get; set; }

        [BsonElement("creatorPubKeyIpfsHash")]
        public virtual string CreatorPubKeyIpfsHash { get; set; }

        [BsonElement("totalSupply")]
        public long TotalSupply { get; set; }


        [BsonElement("edition")]
        public long Edition { get; set; }

        [BsonElement("url")]
        public string URL { get; set; }

        [BsonElement("createdOn")]
        public string CreatedOn { get; set; }


    }
}
