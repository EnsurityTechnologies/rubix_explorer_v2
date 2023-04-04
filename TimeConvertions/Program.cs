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


    class Program
    {
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

            public virtual string block_hash { get; set; }

            public List<string> quorum_list { get; set; }
        }


        static void Main(string[] args)
        {


            var input = new CreateRubixTransactionDto();
            input.amount = 0;
            input.token_time = 0;
            input.token_id = new List<string>();
            input.transaction_id = "test";
            input.rbt_transaction_id = "test";




            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://explorer.rubix.network/api/v2/services/app/Rubix/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsJsonAsync("CreateOrUpdateRubixTransaction", input).Result;

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
