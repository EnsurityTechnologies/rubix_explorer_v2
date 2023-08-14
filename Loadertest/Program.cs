using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Servers;
using MongoDB.Driver.Linq;
using Rubix.API.Shared;
using Rubix.API.Shared.Common;
using Rubix.API.Shared.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Loadertest
{
    class Program
    {

        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        private static string CalculateDifference(DateTime fromData, DateTime toData)
        {
            TimeSpan ts = toData - fromData;
            string diff = string.Format("{0} hours, {1} minutes", ts.Hours, ts.Minutes);

            return diff;
        }

        private MongoClient client = null;
        private IMongoDatabase db = null;

        public static void Main(string[] args)
        {
            try
            {
                var login = "admin";
                var password = Uri.EscapeDataString("IjzUmspU8yDwg5MW");
                var server = "cluster0.jeaxq.mongodb.net";
                MongoClient client = new MongoClient($"mongodb+srv://{login}:{password}@{server}/rubixDb?retryWrites=true&w=majority");

                IMongoDatabase db = client.GetDatabase("rubixDb");



                var tempCollection = db.GetCollection<BsonDocument>("temp_balance");
                var userDidSet = new HashSet<string>();

                //Storing the tempcollection for verification
                 tempCollection.Find(new BsonDocument())
                   .ForEachAsync(doc => userDidSet.Add(doc["user_did"].AsString)).Wait();


                var transactionCollection = db.GetCollection<BsonDocument>("_transactions");

                var tokensCollection = db.GetCollection<BsonDocument>("_tokens");

                var userCollection = db.GetCollection<BsonDocument>("_users");
                var filter = Builders<BsonDocument>.Filter.Empty; // This retrieves all documents

                var documents = userCollection.Find(filter).ToList();
                int count = 1;
                foreach (var document in documents)
                {
                    // Assuming your documents have a field named "fieldName"
                    if (document.Contains("user_did"))
                    {
                        var user_did = document["user_did"].AsString;
                        Console.WriteLine(count+$":user_did: {user_did}");

                        if (userDidSet.Contains(user_did))
                        {
                            Console.WriteLine("User already synced in tempCollection. Skipping...");
                            Console.WriteLine("*************************************************************************");
                            count++;
                            continue;
                        }

                        var senderFilter = Builders<BsonDocument>.Filter.Eq("sender_id", user_did);
                        var senderTransactions = transactionCollection.Find(senderFilter).ToList();
                        decimal senderBalance = 0;

                        foreach (var transaction in senderTransactions)
                        {
                            decimal amount = transaction["amount"].AsDecimal;
                            senderBalance -= amount; // Subtract sent amount from balance
                        }

                        // Calculate balance for the receiver
                        var receiverFilter = Builders<BsonDocument>.Filter.Eq("receiver_id", user_did);
                        var receiverTransactions = transactionCollection.Find(receiverFilter).ToList();
                        decimal receiverBalance = 0;
                        foreach (var transaction in receiverTransactions)
                        {
                            decimal amount = transaction["amount"].AsDecimal;
                            receiverBalance += amount; // Add received amount to balance
                        }

                        decimal balance = receiverBalance - senderBalance;



                        var tokenUserFilter = Builders<BsonDocument>.Filter.Eq("user_did", user_did);
                        var tokensCount = tokensCollection.Find(tokenUserFilter).Count();

                        balance += tokensCount;

                        Console.WriteLine($"balance: {balance}");
                        Console.WriteLine("*************************************************************************");


                        //var updateFilter = Builders<BsonDocument>.Filter.Eq("user_did", document["user_did"]);
                        //var update = Builders<BsonDocument>.Update.Set("balance", balance);

                        //userCollection.UpdateOne(updateFilter, update);



                        var tempDoc = new BsonDocument
                        {
                            { "user_did", document["user_did"] },
                            { "balance", balance },
                            { "isBalanceUpdated", true } // You can adjust this value as needed
                        };

                        tempCollection.InsertOneAsync(tempDoc).Wait();

                        Console.WriteLine("Balance Updated Successfully");
                        Console.WriteLine("*************************************************************************");
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
