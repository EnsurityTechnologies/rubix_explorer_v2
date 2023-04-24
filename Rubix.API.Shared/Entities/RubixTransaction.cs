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
        public RubixTransaction(string transaction_id, string sender_did, string receiver_did, double token_time, double amount, TransactionType transactionType, string nftToken,
        string nftBuyer, string nftSeller, string nftCreatorInput,long totalSupply, long editionNumber,string rbt_transaction_id,string userHash,string blockHash) =>
        (Transaction_id, Sender_did, Receiver_did, Token_time, Amount, TransactionType, NftToken, NftBuyer, NftSeller, NftCreatorInput,TotalSupply,EditionNumber,RBTTransactionId,UserHash,BlockHash) = (transaction_id, sender_did, receiver_did, token_time, amount, transactionType, nftToken, nftBuyer, nftSeller, nftCreatorInput,totalSupply,editionNumber,rbt_transaction_id,userHash,blockHash);

        [BsonElement("transaction_id")]
        public virtual string Transaction_id { get; set; }


        [BsonElement("sender_did")]
        public virtual string Sender_did { get; set; }

        [BsonElement("receiver_did")]
        public virtual string Receiver_did { get; set; }


        [BsonElement("token_time")]
        public virtual double Token_time { get; set; }

        [BsonElement("amount")]
        public virtual double Amount { get; set; }

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

        [BsonElement("block_hash")]
        public virtual string BlockHash { get; set; } 
    }




    public enum TransactionType
    {
        RBT,
        NFT,
        DataToken
    }
}