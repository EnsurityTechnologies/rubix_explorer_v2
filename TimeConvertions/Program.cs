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


    public class CreateNFTTokenInput
    {
        public virtual string type { get; set; }


        public virtual string creatorId { get; set; }


        public virtual string nftToken { get; set; }

        public virtual DateTime createdOn { get; set; }


        public virtual string creatorPubKeyIpfsHash { get; set; }

        public long totalSupply { get; set; }

        public long edition { get; set; }

        public string url { get; set; }

        public string creatorInput { get; set; }
    }


    public class CreatorInput
    {
        public virtual string nftType { get; set; }
        public virtual string color { get; set; }
        public virtual string creatorName { get; set; }
        public virtual string description { get; set; }
        public virtual string blockChain { get; set; }
        public virtual string comment { get; set; }
        public virtual string nftTitle { get; set; }
        public virtual string createdOn { get; set; }
        public virtual string creatorPubKeyIpfsHash { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {


            var creatorInputRequest = new CreatorInput()
            {
                description = "dasdas",
                blockChain = "rubix",
                color = "dasdas",
                comment = "asdasdsa",
                createdOn = DateTime.UtcNow.ToString(),
                creatorName = "raja",
                creatorPubKeyIpfsHash = "dasdas",
                nftTitle = "dasds",
                nftType = "image"
            };

            var creatorInput = JsonConvert.SerializeObject(creatorInputRequest);

            var finalRequestObject = new CreateNFTTokenInput()
            {
                type = "NFT",
               totalSupply=10,
               createdOn=DateTime.Now,
               creatorId="rajasekhar",
               creatorInput= creatorInput,
               creatorPubKeyIpfsHash="dsadsa",
               edition=0,
               nftToken=null,
               url="asdsadas"

            };

           var _apirequest = new RubixCommonInput() {
                 InputString = JsonConvert.SerializeObject(finalRequestObject)
           };

            var finalOutPut = JsonConvert.SerializeObject(_apirequest);


           

            Console.WriteLine(finalOutPut);
        }
    }
}
