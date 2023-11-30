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
    class Programtest
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
                Console.WriteLine("Please enter the range to update the balance");
                Console.WriteLine("Minimum number");
                var min = int.Parse(Console.ReadLine());
                Console.WriteLine("Maximum number");
                var max = int.Parse(Console.ReadLine());
                Console.WriteLine("Status");
                var status = int.Parse(Console.ReadLine());
                var login = "admin";
                var password = Uri.EscapeDataString("IjzUmspU8yDwg5MW");
                var server = "cluster0.jeaxq.mongodb.net";
                MongoClient client = new MongoClient($"mongodb+srv://{login}:{password}@{server}/rubixDb?retryWrites=true&w=majority");

                IMongoDatabase db = client.GetDatabase("rubixDb");
                int count = 0;
                var tempCollection = db.GetCollection<BsonDocument>("temp_balance");

                var users = db.GetCollection<BsonDocument>("_users");
                var filter1 = Builders<BsonDocument>.Filter.Empty; // This retrieves all documents
                                                                   //var tempusers1 = tempCollection.Find(filter1).ToList();
                                                                   //var users1 = users.Find(filter1).ToList();



                var transactionCollection = db.GetCollection<BsonDocument>("_transactions");
                // var transactionRecords = transactionCollection.Find(filter1).ToList();

                var tokensCollection = db.GetCollection<BsonDocument>("_tokens");
                var userDIDMapperDictSet = new Dictionary<string, string>();

                var didMapperCollection = db.GetCollection<BsonDocument>("_didMapperCollection");
                didMapperCollection.Find(new BsonDocument())
                   .ForEachAsync(doc => userDIDMapperDictSet.Add(doc["old_did"].AsString, doc["new_did"].AsString)).Wait();

                var filter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Gte("no", min), // Greater than or equal to 1
                    Builders<BsonDocument>.Filter.Lte("no", max),// Less than or equal to 100
                    Builders<BsonDocument>.Filter.Eq("isBalanceUpdated", status)
                 );

                // Find documents that match the filter
                var results = tempCollection.Find(filter).ToList();


                foreach (var didMapper in results)
                {
                    try
                    {
                        var currentdid = didMapper["did"].AsString;
                        var updateFilter = Builders<BsonDocument>.Filter.Eq("did", currentdid);
                        var update = Builders<BsonDocument>.Update.Set("isBalanceUpdated", 1);
                        tempCollection.UpdateOne(updateFilter, update);


                        var senderFilter = Builders<BsonDocument>.Filter.Eq("sender_id", currentdid);
                        var senderTransactions = transactionCollection.Find(senderFilter).ToList();
                        decimal senderBalance = 0;

                        foreach (var transaction in senderTransactions)
                        {
                            decimal amount = transaction["amount"].AsDecimal;
                            senderBalance -= amount; // Subtract sent amount from balance
                        }

                        // Calculate balance for the receiver
                        var receiverFilter = Builders<BsonDocument>.Filter.Eq("receiver_id", currentdid);
                        var receiverTransactions = transactionCollection.Find(receiverFilter).ToList();
                        decimal receiverBalance = 0;
                        foreach (var transaction in receiverTransactions)
                        {
                            decimal amount = transaction["amount"].AsDecimal;
                            receiverBalance += amount; // Add received amount to balance
                        }

                        decimal balance = receiverBalance - senderBalance;



                        var tokenUserFilter = Builders<BsonDocument>.Filter.Eq("user_did", currentdid);
                        var tokensCount = tokensCollection.Find(tokenUserFilter).Count();

                        balance += tokensCount;

                        if (userDIDMapperDictSet.ContainsKey(currentdid))
                        {
                            // Key exists, retrieve the value
                            string newDID = userDIDMapperDictSet[currentdid];

                            var newsenderFilter = Builders<BsonDocument>.Filter.Eq("sender_id", newDID);
                            var newsenderTransactions = transactionCollection.Find(senderFilter).ToList();
                            decimal newsenderBalance = 0;

                            foreach (var transaction in senderTransactions)
                            {
                                decimal amount = transaction["amount"].AsDecimal;
                                newsenderBalance -= amount; // Subtract sent amount from balance
                            }

                            // Calculate balance for the receiver
                            var newreceiverFilter = Builders<BsonDocument>.Filter.Eq("receiver_id", newDID);
                            var newreceiverTransactions = transactionCollection.Find(receiverFilter).ToList();
                            decimal newreceiverBalance = 0;
                            foreach (var transaction in receiverTransactions)
                            {
                                decimal amount = transaction["amount"].AsDecimal;
                                newreceiverBalance += amount; // Add received amount to balance
                            }

                            balance += newreceiverBalance - newsenderBalance;



                            var newtokenUserFilter = Builders<BsonDocument>.Filter.Eq("user_did", newDID);
                            var newtokensCount = tokensCollection.Find(newtokenUserFilter).Count();

                            balance += newtokensCount;

                        }
                        else
                        {
                            Console.WriteLine("No new DID for this user");
                        }
                        Console.WriteLine($"balance: {balance}");
                        //Console.WriteLine("*************************************************************************");

                        var updateFilter1 = Builders<BsonDocument>.Filter.Eq("did", currentdid);
                        var update1 = Builders<BsonDocument>.Update.Set("isBalanceUpdated", 2).Set("balance", balance);
                        tempCollection.UpdateOne(updateFilter1, update1);

                        var userfilter = Builders<BsonDocument>.Filter.Eq("user_did", currentdid);
                        var userupdate = Builders<BsonDocument>.Update.Set("balance", balance);
                        users.UpdateOne(userfilter, update1);

                    }
                    catch (Exception)
                    {


                    }

                }




                //foreach (var document in users1)
                //{
                //    // Assuming your documents have a field named "fieldName"

                //    if (document.Contains("user_did"))
                //    {
                //        var user_did = document["user_did"].AsString;
                //        Console.WriteLine(count + $":user_did: {user_did}");

                //        if (tempusers1.Contains(user_did))
                //        {
                //            Console.WriteLine("User already synced in tempCollection. Skipping...");
                //            Console.WriteLine("*************************************************************************");
                //            count++;
                //            continue;
                //        }

                //        var senderFilter = Builders<BsonDocument>.Filter.Eq("sender_id", user_did);
                //        var senderTransactions = transactionCollection.Find(senderFilter).ToList();
                //        decimal senderBalance = 0;

                //        foreach (var transaction in senderTransactions)
                //        {
                //            decimal amount = transaction["amount"].AsDecimal;
                //            senderBalance -= amount; // Subtract sent amount from balance
                //        }

                //        // Calculate balance for the receiver
                //        var receiverFilter = Builders<BsonDocument>.Filter.Eq("receiver_id", user_did);
                //        var receiverTransactions = transactionCollection.Find(receiverFilter).ToList();
                //        decimal receiverBalance = 0;
                //        foreach (var transaction in receiverTransactions)
                //        {
                //            decimal amount = transaction["amount"].AsDecimal;
                //            receiverBalance += amount; // Add received amount to balance
                //        }

                //        decimal balance = receiverBalance - senderBalance;



                //        var tokenUserFilter = Builders<BsonDocument>.Filter.Eq("user_did", user_did);
                //        var tokensCount = tokensCollection.Find(tokenUserFilter).Count();

                //        balance += tokensCount;

                //        if (userDIDMapperDictSet.ContainsKey(user_did))
                //        {
                //            // Key exists, retrieve the value
                //            string newDID = userDIDMapperDictSet[user_did];

                //            var newsenderFilter = Builders<BsonDocument>.Filter.Eq("sender_id", newDID);
                //            var newsenderTransactions = transactionCollection.Find(senderFilter).ToList();
                //            decimal newsenderBalance = 0;

                //            foreach (var transaction in senderTransactions)
                //            {
                //                decimal amount = transaction["amount"].AsDecimal;
                //                newsenderBalance -= amount; // Subtract sent amount from balance
                //            }

                //            // Calculate balance for the receiver
                //            var newreceiverFilter = Builders<BsonDocument>.Filter.Eq("receiver_id", newDID);
                //            var newreceiverTransactions = transactionCollection.Find(receiverFilter).ToList();
                //            decimal newreceiverBalance = 0;
                //            foreach (var transaction in receiverTransactions)
                //            {
                //                decimal amount = transaction["amount"].AsDecimal;
                //                newreceiverBalance += amount; // Add received amount to balance
                //            }

                //            balance += newreceiverBalance - newsenderBalance;



                //            var newtokenUserFilter = Builders<BsonDocument>.Filter.Eq("user_did", newDID);
                //            var newtokensCount = tokensCollection.Find(newtokenUserFilter).Count();

                //            balance += newtokensCount;

                //        }
                //        else
                //        {
                //            Console.WriteLine("No new DID for this user");
                //        }
                //        Console.WriteLine($"balance: {balance}");
                //        Console.WriteLine("*************************************************************************");


                //        var updateFilter = Builders<BsonDocument>.Filter.Eq("user_did", document["user_did"]);
                //        var update = Builders<BsonDocument>.Update.Set("balance", balance);

                //        user.UpdateOne(updateFilter, update);

                //        var tempDoc = new BsonDocument
                //            {
                //                { "user_did", document["user_did"] },
                //                { "balance", balance },
                //                { "isBalanceUpdated", true } // You can adjust this value as needed
                //            };

                //        tempCollection.InsertOneAsync(tempDoc).Wait();

                //        Console.WriteLine("Balance Updated Successfully");
                //        Console.WriteLine("*************************************************************************");
                //        count++;
                //    }
                //}


                //int count = 0;
                //var tempcol = new List<BsonDocument>();
                //int totalcounter = 0;
                //int tempcont = 2000;
                //foreach (var user in users1)
                //{

                //    count++;
                //    totalcounter++;
                //    var tempDoc = new BsonDocument
                //    {
                //        {"no",totalcounter },
                //        { "did",user["user_did"].AsString },
                //        { "balance", 0 },
                //        { "isBalanceUpdated", 0 } // You can adjust this value as needed
                //    };
                //    tempcol.Add(tempDoc);
                //    if (count == tempcont)
                //    {
                //        tempCollection.InsertManyAsync(tempcol).Wait();
                //        tempcol = new List<BsonDocument>();
                //        count = 0;
                //    }
                //    if (totalcounter == 170000)
                //    {
                //        tempcont = 99;
                //    }




                //    //tempCollection.UpdateMany

                //}

                //step1: Copy all user did to the temp table - did, balance and status 
                //step2: Take 25 records at a time from the table with status - update the status from none -0 to processing -1
                //step3: Calculate balance for each record and update the status to completed -2





                //var userDidSet = new HashSet<string>();

                //    var userDIDMapperDictSet=new Dictionary<string,string>();

                //    //Storing the tempcollection for verification
                //     tempCollection.Find(new BsonDocument())
                //       .ForEachAsync(doc => userDidSet.Add(doc["user_did"].AsString)).Wait();


                //    var transactionCollection = db.GetCollection<BsonDocument>("_transactions");

                //    var tokensCollection = db.GetCollection<BsonDocument>("_tokens");

                //    var didMapperCollection = db.GetCollection<BsonDocument>("_didMapperCollection");
                //    didMapperCollection.Find(new BsonDocument())
                //      .ForEachAsync(doc => userDIDMapperDictSet.Add(doc["old_did"].AsString, doc["new_did"].AsString)).Wait();



                //    var userCollection = db.GetCollection<BsonDocument>("_users");
                //    var filter = Builders<BsonDocument>.Filter.Empty; // This retrieves all documents

                //    var documents = userCollection.Find(filter).ToList();
                //    int count = 1;
                //    Console.WriteLine("Total DID's: "+documents.Count());
                //    foreach (var document in documents)
                //    {
                //        // Assuming your documents have a field named "fieldName"
                //        if (document.Contains("user_did"))
                //        {
                //            var user_did = document["user_did"].AsString;
                //            Console.WriteLine(count+$":user_did: {user_did}");

                //            if (userDidSet.Contains(user_did))
                //            {
                //                Console.WriteLine("User already synced in tempCollection. Skipping...");
                //                Console.WriteLine("*************************************************************************");
                //                count++;
                //                continue;
                //            }

                //            var senderFilter = Builders<BsonDocument>.Filter.Eq("sender_id", user_did);
                //            var senderTransactions = transactionCollection.Find(senderFilter).ToList();
                //            decimal senderBalance = 0;

                //            foreach (var transaction in senderTransactions)
                //            {
                //                decimal amount = transaction["amount"].AsDecimal;
                //                senderBalance -= amount; // Subtract sent amount from balance
                //            }

                //            // Calculate balance for the receiver
                //            var receiverFilter = Builders<BsonDocument>.Filter.Eq("receiver_id", user_did);
                //            var receiverTransactions = transactionCollection.Find(receiverFilter).ToList();
                //            decimal receiverBalance = 0;
                //            foreach (var transaction in receiverTransactions)
                //            {
                //                decimal amount = transaction["amount"].AsDecimal;
                //                receiverBalance += amount; // Add received amount to balance
                //            }

                //            decimal balance = receiverBalance - senderBalance;



                //            var tokenUserFilter = Builders<BsonDocument>.Filter.Eq("user_did", user_did);
                //            var tokensCount = tokensCollection.Find(tokenUserFilter).Count();

                //            balance += tokensCount;

                //            if (userDIDMapperDictSet.ContainsKey(user_did))
                //            {
                //                // Key exists, retrieve the value
                //                string newDID = userDIDMapperDictSet[user_did];

                //                var newsenderFilter = Builders<BsonDocument>.Filter.Eq("sender_id", newDID);
                //                var newsenderTransactions = transactionCollection.Find(senderFilter).ToList();
                //                decimal newsenderBalance = 0;

                //                foreach (var transaction in senderTransactions)
                //                {
                //                    decimal amount = transaction["amount"].AsDecimal;
                //                    newsenderBalance -= amount; // Subtract sent amount from balance
                //                }

                //                // Calculate balance for the receiver
                //                var newreceiverFilter = Builders<BsonDocument>.Filter.Eq("receiver_id", newDID);
                //                var newreceiverTransactions = transactionCollection.Find(receiverFilter).ToList();
                //                decimal newreceiverBalance = 0;
                //                foreach (var transaction in receiverTransactions)
                //                {
                //                    decimal amount = transaction["amount"].AsDecimal;
                //                    newreceiverBalance += amount; // Add received amount to balance
                //                }

                //                 balance += newreceiverBalance - newsenderBalance;



                //                var newtokenUserFilter = Builders<BsonDocument>.Filter.Eq("user_did", newDID); 
                //                var newtokensCount = tokensCollection.Find(newtokenUserFilter).Count(); 

                //                balance += newtokensCount;

                //            }
                //            else
                //            {
                //                Console.WriteLine("No new DID for this user");
                //            }
                //            Console.WriteLine($"balance: {balance}");
                //            Console.WriteLine("*************************************************************************");


                //            var updateFilter = Builders<BsonDocument>.Filter.Eq("user_did", document["user_did"]);
                //            var update = Builders<BsonDocument>.Update.Set("balance", balance);

                //            userCollection.UpdateOne(updateFilter, update);

                //            var tempDoc = new BsonDocument
                //            {
                //                { "user_did", document["user_did"] },
                //                { "balance", balance },
                //                { "isBalanceUpdated", true } // You can adjust this value as needed
                //            };

                //            tempCollection.InsertOneAsync(tempDoc).Wait();

                //            Console.WriteLine("Balance Updated Successfully");
                //            Console.WriteLine("*************************************************************************");
                //            count++;
                //        }
                //    }

                //    Console.WriteLine("************************************************************");
                //    Console.WriteLine("All DID Balances Updated");
                //    Console.WriteLine("************************************************************");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
