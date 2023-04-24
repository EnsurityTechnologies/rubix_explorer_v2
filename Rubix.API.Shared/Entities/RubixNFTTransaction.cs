using MongoDB.Bson.Serialization.Attributes;
using Rubix.API.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Entities
{
    public class RubixNFTTransaction : BaseEntity
    {
        public RubixNFTTransaction(TransactionType transactionType, string nftToken,
        string nftBuyer, string nftSeller, string nftCreatorInput, long totalSupply, long editionNumber, string rbt_transaction_id, string userHash) =>
        (TransactionType, NftToken, NftBuyer, NftSeller, NftCreatorInput, TotalSupply, EditionNumber, RBTTransactionId, UserHash) = (transactionType, nftToken, nftBuyer, nftSeller, nftCreatorInput, totalSupply, editionNumber, rbt_transaction_id, userHash);


        [BsonElement("transaction_type")]
        public virtual TransactionType TransactionType { get; set; }

        [BsonElement("nftToken")]
        public virtual string NftToken { get; set; }

        [BsonElement("nftBuyer")]
        public virtual string NftBuyer { get; set; }

        [BsonElement("nftSeller")]
        public virtual string NftSeller { get; set; }

        [BsonElement("nftCreatorInput")]
        public virtual string NftCreatorInput { get; set; }

        [BsonElement("totalSupply")]
        public virtual long TotalSupply { get; set; }

        [BsonElement("editionNumber")]
        public virtual long EditionNumber { get; set; }

        [BsonElement("rbt_transaction_id")]
        public virtual string RBTTransactionId { get; set; }

        [BsonElement("userHash")]
        public virtual string UserHash { get; set; }

       // [BsonElement("block_hash")]
       // public virtual string BlockHash { get; set; }
    }

}
