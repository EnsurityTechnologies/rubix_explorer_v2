using Newtonsoft.Json;
using Rubix.API.Shared;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Enums;
using System;
using System.Collections.Generic;

namespace TimeConvertions
{
    public class RubixCommonInput
    {
        public string InputString { get; set; }
    }
    public class CreateRubixTransactionDto
    {
        public string transaction_id { get; set; }
        public string sender_did { get; set; }
        public string receiver_did { get; set; }
        public double token_time { get; set; }
        public List<string> token_id { get; set; }
        public double amount { get; set; }
        public TransactionType transaction_type { get; set; }
        public string nftToken { get; set; }
        public string nftBuyer { get; set; }
        public string nftSeller { get; set; }
        public string nftCreatorInput { get; set; }
        public virtual long totalSupply { get; set; }
        public virtual long editionNumber { get; set; }
        public virtual string rbt_transaction_id { get; set; }
        public virtual string userHash { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var obj = new CreateRubixTransactionDto()
            {
                transaction_id= "7007ad4cb6648621a6d00637a718796c952bf3113411f9cfd1f4583def4f70c4",
                amount=0.012,
                nftCreatorInput="",
                totalSupply=4,
                rbt_transaction_id= "09ba0f3887447e3a91b0e4e374a017fdd817d2f34c65b9cb4c9653f09a4c2507",
                receiver_did= "QmbpbXUkwF73t9n2unpnPXrT1bJraoG5h3mTxC28tXW5Ci",
                transaction_type=TransactionType.DataToken,
                nftSeller= "QmbpbXUkwF73t9n2unpnPXrT1bJraoG5h3mTxC28tXW5Ci",
                editionNumber=1,
                sender_did= "QmaLfno6jkVPtgPgLrSUv9HDK3Ft9dS6wmRDT3agxuphVJ",
                token_time= 129186,
                nftToken= "QmWs2T3uTPDwFhKvZgigNUAbaSN9XecwGPuh3vWajTjTzy",
                nftBuyer= "QmaLfno6jkVPtgPgLrSUv9HDK3Ft9dS6wmRDT3agxuphVJ",
                token_id= new List<string> { "QmWHgZLMYKDHpuhrCLpSAsEsjd97RXGWcw1xvN2nEhdePN" }

            };

           var test = new RubixCommonInput() {
                 InputString = JsonConvert.SerializeObject(obj)
           };

            var finalOutPut = JsonConvert.SerializeObject(test);
            Console.WriteLine(finalOutPut);
        }
    }
}
