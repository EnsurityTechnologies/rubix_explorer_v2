using Newtonsoft.Json;
using Rubix.API.Shared;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

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
        public class CreateDataTokenDto
        {
            public string transaction_id { get; set; }
            public string commiter { get; set; }
            public string sender { get; set; }
            public string receiver { get; set; }
            public double token_time { get; set; }
            public double amount { get; set; }
            public TransactionType transaction_type { get; set; }
            public virtual string rbt_transaction_id { get; set; }

            public Dictionary<string, float> quorum_list { get; set; }
            public Dictionary<string, string> datatokens { get; set; }
        }


        static void Main(string[] args)
        {



            var tokens = new Dictionary<string, float>();
            tokens.TryAdd(Guid.NewGuid().ToString(), 10);
            tokens.TryAdd(Guid.NewGuid().ToString(), 20);
            tokens.TryAdd(Guid.NewGuid().ToString(), 30);


            var quorumList = new Dictionary<string, string>();
            quorumList.TryAdd(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            quorumList.TryAdd(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            quorumList.TryAdd(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            var input = new CreateDataTokenDto();
            input.token_time = 3.0;
            input.transaction_id= Guid.NewGuid().ToString();
            input.sender= Guid.NewGuid().ToString();
            input.receiver= Guid.NewGuid().ToString();
            input.amount= 100;
            input.quorum_list = tokens;
            input.datatokens= quorumList;
            input.commiter= Guid.NewGuid().ToString();
            input.rbt_transaction_id= Guid.NewGuid().ToString();
            input.receiver= Guid.NewGuid().ToString();



            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://explorer.rubix.network/api/v2/services/app/Rubix/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsJsonAsync("create-datatokens", input).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        // Get the URI of the created resource.
                        Uri returnUrl = response.Headers.Location;
                        Console.WriteLine(returnUrl);
                    }
                }
            }
            catch(Exception ex)
            {

            }
            

        }
    }
}
