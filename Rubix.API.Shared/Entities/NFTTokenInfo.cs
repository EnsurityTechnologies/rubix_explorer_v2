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
        public NFTTokenInfo(string token_type, string creator_id, string nft_token,List<string> creator_public_ipfs_hash,long total_supply,long edition,string url,string creator_input) =>
            (TokenType, CreatorId, NFTToken, CreatorPublicIPFSHash,TotalSupply, Edition, URL, CreatorInput) = (token_type, creator_id, nft_token, creator_public_ipfs_hash, total_supply, edition,url,creator_input);


        [BsonElement("token_type")]
        public virtual string TokenType { get; set; } 

        [BsonElement("creator_id")]
        public virtual string CreatorId { get; set; }
       
        [BsonElement("nft_token")]

        public virtual string NFTToken { get; set; }

        [BsonElement("creator_public_ipfs_hash")]
        public virtual List<string> CreatorPublicIPFSHash { get; set; }

        [BsonElement("total_supply")]
        public long TotalSupply { get; set; }


        [BsonElement("edition")]
        public long Edition { get; set; }

        [BsonElement("url")]
        public string URL { get; set; }

        [BsonElement("creator_input")]
        public string CreatorInput { get; set; }

    }
}
