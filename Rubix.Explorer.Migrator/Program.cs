using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Interfaces;
using Rubix.API.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.Explorer.Migrator
{
    class Program
    {


        #region  ----------- Start  Sql Connectionstring and Sql Queries ------------------

        public const string dbConnectionString = "Server=rubixexplorer.database.windows.net,1433;Initial Catalog=RubixExpDashboard;Persist Security Info=False;User ID=rubixexplorer;Password=58Wa5yEAa#L3mgPx;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public const string usersQuery = "SELECT CreationTime,LastModificationTime,user_did,peerid,ipaddress,balance FROM RubixUsers";
        public const string tokensQuery = "SELECT CreationTime,LastModificationTime,token_id,bank_id,denomination,user_did,level FROM RubixTokens";
        public const string transactionsQuery = "SELECT CreationTime,LastModificationTime,transaction_id,sender_did,receiver_did,token_time,amount FROM RubixTransactions";
        public const string tokenTrnsactionsQuery = "SELECT CreationTime,LastModificationTime,transaction_id,token_id FROM RubixTokenTransactions";


        #endregion  ---------------- End  Sql Connectionstring and Sql Queries  -----------------------

        public static Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();

            LoadUsers(host.Services).Wait();
            //LoadTokens(host.Services).Wait();
           // LoadTransactions(host.Services).Wait();
           // LoadTokenTransactions(host.Services).Wait();

            Console.WriteLine("Completed the Task");
            return  host.RunAsync();

        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) => {
                    services.AddSingleton<IMongoClient>(c =>
                    {
                        var login = "admin";
                        var password = Uri.EscapeDataString("DtfeJS0G5vfUtNWI");
                        var server = "cluster0.peyce.mongodb.net";
                        // return new MongoClient("mongodb://rubixdb:kVA6oR6z3nJvhaoXCg3vJ04WUbTkt10mnhBV1E1Fgq2wEzpnRe5LLPnxu6Dr4z8CMHlCOvHBLhbu3LfrlySc6g==@rubixdb.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@rubixdb@");

                        return new MongoClient($"mongodb+srv://{login}:{password}@{server}/rubixDb?retryWrites=true&w=majority");
                    });

                    services.AddScoped(c =>
                        c.GetService<IMongoClient>().StartSession());

                    services.AddTransient<IRepositoryRubixUser, RepositoryRubixUser>();
                    services.AddTransient<IRepositoryRubixToken, RepositoryRubixToken>();
                    services.AddTransient<IRepositoryRubixTokenTransaction, RepositoryRubixTokenTransaction>();
                    services.AddTransient<IRepositoryRubixTransaction, RepositoryRubixTransaction>();

                });
        }


        #region -------- Start ---------------

        public static async Task LoadUsers(IServiceProvider services) 
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var userRepo = provider.GetRequiredService<IRepositoryRubixUser>();


            List<RubixUser> rubixUsers = new List<RubixUser>();
            RubixUser rubixUser =null;

            SqlConnection con = new SqlConnection(dbConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(usersQuery, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                var userdid = dr["user_did"].ToString();
                var peerid = dr["peerid"].ToString();
                var ipAddress = dr["ipaddress"].ToString();
                var balance = Convert.ToInt32(dr["balance"]);

                rubixUser = new RubixUser(userdid, peerid, ipAddress,balance);
                var creationTime = dr["CreationTime"].ToString();
                if(!string.IsNullOrEmpty(creationTime))
                {
                    rubixUser.CreationTime = Convert.ToDateTime(creationTime);
                }


                var modificationTime = dr["LastModificationTime"].ToString();
                if (!string.IsNullOrEmpty(modificationTime))
                {
                    rubixUser.LastModificationTime = Convert.ToDateTime(modificationTime);
                }
                rubixUsers.Add(rubixUser);

                Console.WriteLine("Users: " + rubixUsers.Count());
            }

            //Insert the all records into mongo db
             await userRepo.InsertManyAsync(rubixUsers);
             Console.WriteLine("Completed the Users Syncing");
        }


        public static async Task LoadTokens(IServiceProvider services) 
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var tokensRepo = provider.GetRequiredService<IRepositoryRubixToken>();


            List<RubixToken> rubixTokens = new List<RubixToken>();
            RubixToken rubixToken = null; 

            SqlConnection con = new SqlConnection(dbConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(tokensQuery, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                var token_id = dr["token_id"].ToString();
                var bank_id = dr["bank_id"].ToString();
                var denomination =Convert.ToInt32(dr["denomination"].ToString());
                var user_did = dr["user_did"].ToString();
                var level = dr["level"].ToString();
                rubixToken = new RubixToken(token_id, bank_id, denomination, user_did,level);
                var creationTime = dr["CreationTime"].ToString();
                if (!string.IsNullOrEmpty(creationTime))
                {
                    rubixToken.CreationTime = Convert.ToDateTime(creationTime);
                }
                var modificationTime = dr["LastModificationTime"].ToString();
                if (!string.IsNullOrEmpty(modificationTime))
                {
                    rubixToken.LastModificationTime = Convert.ToDateTime(modificationTime);
                }
                rubixTokens.Add(rubixToken);
                Console.WriteLine("Tokens: "+ rubixTokens.Count());
            }

            //Insert the all records into mongo db
             await tokensRepo.InsertManyAsync(rubixTokens);

            Console.WriteLine("Completed the Tokens Syncing");
        }


        public static async Task LoadTransactions(IServiceProvider services) 
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var rubixTransactionsRepo = provider.GetRequiredService<IRepositoryRubixTransaction>();


            List<RubixTransaction> rubixTransactions = new List<RubixTransaction>();
            RubixTransaction rubixTransaction = null;

            SqlConnection con = new SqlConnection(dbConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(transactionsQuery, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                var transaction_id = dr["transaction_id"].ToString();
                var sender_did = dr["sender_did"].ToString();
                var receiver_did = dr["receiver_did"].ToString();
                var token_time = Convert.ToDouble(dr["token_time"].ToString());
                var amount =Convert.ToInt32(dr["amount"].ToString());

                rubixTransaction = new RubixTransaction(transaction_id, sender_did, receiver_did, token_time, amount);
                var creationTime = dr["CreationTime"].ToString();
                if (!string.IsNullOrEmpty(creationTime))
                {
                    rubixTransaction.CreationTime = Convert.ToDateTime(creationTime);
                }
                var modificationTime = dr["LastModificationTime"].ToString();
                if (!string.IsNullOrEmpty(modificationTime))
                {
                    rubixTransaction.LastModificationTime = Convert.ToDateTime(modificationTime);
                }
                rubixTransactions.Add(rubixTransaction);
                Console.WriteLine("Transaction: " + rubixTransactions.Count());
            }

            //Insert the all records into mongo db
             await rubixTransactionsRepo.InsertManyAsync(rubixTransactions);

            Console.WriteLine("Completed the Transactions Syncing");
        }


        public static async Task LoadTokenTransactions(IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var rubixTokenTransactionsRepo = provider.GetRequiredService<IRepositoryRubixTokenTransaction>();


            List<RubixTokenTransaction> rubixTokenTransactions = new List<RubixTokenTransaction>();
            RubixTokenTransaction rubixTokenTransaction = null;

            SqlConnection con = new SqlConnection(dbConnectionString);
          
            con.Open();
            SqlCommand cmd = new SqlCommand(tokenTrnsactionsQuery, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                var transaction_id = dr["transaction_id"].ToString();
                var token_id = dr["token_id"].ToString();

                rubixTokenTransaction = new RubixTokenTransaction(transaction_id, token_id);

                var creationTime = dr["CreationTime"].ToString();
                if (!string.IsNullOrEmpty(creationTime))
                {
                    rubixTokenTransaction.CreationTime = Convert.ToDateTime(creationTime);
                }
                var modificationTime = dr["LastModificationTime"].ToString();
                if (!string.IsNullOrEmpty(modificationTime))
                {
                    rubixTokenTransaction.LastModificationTime = Convert.ToDateTime(modificationTime);
                }
                rubixTokenTransactions.Add(rubixTokenTransaction);
                Console.WriteLine("Token Transaction: " + rubixTokenTransactions.Count());
            }
            //Insert the all records into mongo db
            await rubixTokenTransactionsRepo.InsertManyAsync(rubixTokenTransactions);

            Console.WriteLine("Completed the Token Transactions Syncing");
        }

        #endregion ----------- End -----------

    }
}
