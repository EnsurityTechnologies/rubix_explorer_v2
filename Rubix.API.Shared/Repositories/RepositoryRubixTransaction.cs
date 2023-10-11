using MongoDB.Bson;
using MongoDB.Driver;
using Rubix.API.Shared.Common;
using Rubix.API.Shared.Dto;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Interfaces;
using Rubix.API.Shared.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Repositories
{
    public class RepositoryRubixTransaction : BaseRepository<RubixTransaction>, IRepositoryRubixTransaction
    {
        public RepositoryRubixTransaction(
            IMongoClient mongoClient,
            IClientSessionHandle clientSessionHandle) : base(mongoClient, clientSessionHandle, "_transactions")
        {

        }


        public async Task<RubixTransaction> FindByTransIdAsync(string transId)
        {
            return  Collection.AsQueryable().Where(x => x.Transaction_id == transId).FirstOrDefault();
        }


        public override async Task InsertAsync(RubixTransaction obj)
        {
            await Collection.Indexes.CreateOneAsync(new CreateIndexModel<RubixTransaction>(Builders<RubixTransaction>.IndexKeys.Descending(d => d.Transaction_id), new CreateIndexOptions { Unique = true }));
            await base.InsertAsync(obj);
        }

        public virtual async Task<PageResultDto<TransactionDto>> GetPagedResultAsync(int page, int pageSize)
        {

            var filter = Builders<RubixTransaction>.Filter.Empty;
            var count = await Collection.Find(filter).CountAsync();
            var list = await Collection.Find(filter).SortByDescending(x=>x.CreationTime).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

            List<TransactionDto> targetList = new List<TransactionDto>();
           
            targetList.AddRange(list.Select(item => new TransactionDto()
            {
                amount = item.Amount,
                token_time = Math.Round((item.Token_time) / 1000, 3),
                receiver_did = item.Receiver_did,
                transaction_id = item.Transaction_id,
                sender_did = item.Sender_did,
                time = item.Token_time,
                transaction_fee = 0,
                CreationTime = item.CreationTime,
            }));
            return new PageResultDto<TransactionDto>
            {
                Count = (int)count / pageSize,
                Size = pageSize,
                Page = page,
                Items = targetList
            };
        }


        public virtual async Task<PageResultDto<TransactionDto>> GetPagedResultByDIDAsync(string did,int page, int pageSize) 
        {
            var filter = Builders<RubixTransaction>.Filter.Eq(x => x.Sender_did, did) | Builders<RubixTransaction>.Filter.Eq(x => x.Receiver_did, did);
           
            var count = await Collection.Find(filter).CountAsync();

            var list = await Collection.Find(filter).SortByDescending(x => x.CreationTime).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

            List<TransactionDto> targetList = new List<TransactionDto>();

            targetList.AddRange(list.Select(item => new TransactionDto()
            {
                amount = item.Amount,
                token_time = Math.Round((item.Token_time / item.Amount) / 1000, 3),
                receiver_did = item.Receiver_did,
                transaction_id = item.Transaction_id,
                sender_did = item.Sender_did,
                time = item.Token_time,
                transaction_fee = 0,
                CreationTime=item.CreationTime,
            }));
            return new PageResultDto<TransactionDto>
            {
                Count = (int)count / pageSize,
                Size = pageSize,
                Page = page,
                Items = targetList
            };
        }

        public virtual async Task<PageResultDto<TopWalletsDto>> GetTopWalletsAsync()
        {
            try
            {
                var topWalletList = new List<TopWalletsDto>() {
                new TopWalletsDto(){ DIDOrWalletID = "QmW9C7uJ32AbRqyYjjevLomLsJDdhYwQNVLZQGaeEecnvU", Balance=93202},
                new TopWalletsDto(){ DIDOrWalletID = "QmW8e8XMd9gmkGgxgsgPWEhkKLuE4o4TFhvKXyLAELBRYu", Balance=93042},
                new TopWalletsDto(){ DIDOrWalletID = "Qmb243Uj9dTVaekzi6PuzAkeBVcpxBKKwFjx4tb59BLoxq", Balance=92722},
                new TopWalletsDto(){ DIDOrWalletID = "QmdBvz7KkEFdzimBFGSBEqiYzM31CZWphc1E1TH3Xfd8G7", Balance=92642},
                new TopWalletsDto(){ DIDOrWalletID = "QmRSR166SYfGVYTfFpR7GcHUTExSuKa7WBTa316GG1YnWF", Balance=92640},
                new TopWalletsDto(){ DIDOrWalletID = "QmTZsNaviedbFmosRee8vEYN3kLfb5H7AGTBBmZmbhF16y", Balance=92402},
                new TopWalletsDto(){ DIDOrWalletID = "QmR4ZqZWofMK38QN5YAYVabVHYbXuH5VS3DCJyw56RggE9", Balance=93202},
                new TopWalletsDto(){ DIDOrWalletID = "QmRaSt54trvKnUD9yvnp1vmTjWoixRTtm74YekmAz4rEuF", Balance = 92242},
                new TopWalletsDto(){ DIDOrWalletID = "QmVUgb8JKZXLdkF9i3v8Jh9EG5vTti8WN62Q1GeyhoHhvv", Balance = 92240},
                new TopWalletsDto(){ DIDOrWalletID = "QmZNUWb6EoeeLFrpd7m6oaKkz4HNeMSMHEEKf5ejsfi3No", Balance = 92080},
                new TopWalletsDto(){ DIDOrWalletID = "QmZDLgpiCBsU9Dzg1zdxxMQUuKq3SRj5HDsgcuzk3C8DfD", Balance = 91737},
                new TopWalletsDto(){ DIDOrWalletID = "QmWgrceRN54GtMi4jAKty6yMtC1JktwSJsVbrSWpTNdVVH", Balance = 90082},
                new TopWalletsDto(){ DIDOrWalletID = "QmUP1pDXtSBai2yjgiKWnwEgqGnKoR1t3kswJx65MiQuT5", Balance = 89922},
                new TopWalletsDto(){ DIDOrWalletID = "QmeeA5hwMhc1YzRaC9Q2iqc8K94uNgrxrzEvZHdDGTvgwT", Balance = 89760},
                new TopWalletsDto(){ DIDOrWalletID = "QmajdgcgTtPFLhkxn8mASp7U6yrX9ngYrPdhY8CPqQZks7", Balance = 89040},
                new TopWalletsDto(){ DIDOrWalletID = "QmPvL6cC2RakzdCGw1kCZcSr77u2jsuxHqjbfahvVS1Jhf", Balance = 88161},
                new TopWalletsDto(){ DIDOrWalletID = "QmecVZ5YNYu1oujdDE23nT8HFsvZxDPtUGYv8uUH2tuBmr", Balance = 87602},
                new TopWalletsDto(){ DIDOrWalletID = "QmQbNGcdsPkPwkYwrioxG4EKDSKr2yFikpiist4TZia1XM", Balance = 87359},
                new TopWalletsDto(){ DIDOrWalletID = "QmZyuUZrpbbcsbFJ663zWbSTHs1vZiQpg7YAtTp2Vbdr4z", Balance = 86961},
                new TopWalletsDto(){ DIDOrWalletID = "QmZK1DWnJGuDU9arFYzUjcRQVuMxF1CPgU1aE8p8sJC2XK", Balance = 86914},
                new TopWalletsDto(){ DIDOrWalletID = "QmTKJDUis973hgcisXFx89Cxqy3XjACuzvGPbJRt3oTv56", Balance = 86162},
                new TopWalletsDto(){ DIDOrWalletID = "QmPoY5ovRKErYX1YnL8QJhvS5bshBcujGk9c9sSuMqB6bp", Balance = 85920},
                new TopWalletsDto(){ DIDOrWalletID = "QmWswPqVh8CEfTBrtVKKXXiNKiSaVaFPskF95DKuEHMXxp", Balance = 85842},
                new TopWalletsDto(){ DIDOrWalletID = "QmQqbyZtk6pJ8QERRmkG2eCXmeTt3uVVHjbskZCUPGPXJg", Balance = 85442},
                new TopWalletsDto(){ DIDOrWalletID = "QmfWAwbWmR51k8gV1AY12qMV3X9h6SthmESCQm9VwPFVLx", Balance = 85360},
                new TopWalletsDto(){ DIDOrWalletID = "QmbscdrSsVjvBEeh5m33ceAiQTtHMDWrqtRNuqi5RG9pLf", Balance = 85120},
                new TopWalletsDto(){ DIDOrWalletID = "QmaAiBqwRoQEH6TSi4KmtZ7EbP9Hadk53cSUpC56NXtjqu", Balance = 84263},
                new TopWalletsDto(){ DIDOrWalletID = "QmV8QChZF2pFihFdHQfZgy72HvB2ZXzQqgYxaFeZJ6PieY", Balance = 84113},
                new TopWalletsDto(){ DIDOrWalletID = "QmczmsX1scP6xu84Uf1eEBS1RRry8tgbZdDDWd2o6jDt3U", Balance = 82960},
                new TopWalletsDto(){ DIDOrWalletID = "QmSqzX7JQXk3NkLk561YSt3a3ayGjR7SeNnasbPUmc1cCL", Balance = 75280},
                new TopWalletsDto(){ DIDOrWalletID = "QmX4MAm9miSnNArJFNmJLC5HDVKLnF5uCVg4AmhtjrXuNr", Balance = 64882},
                new TopWalletsDto(){ DIDOrWalletID = "QmUpw8Lqx5MLWWcjdSrrWvgS3etmwHjtXJRXm7iuBcXhbJ", Balance = 64477},
                new TopWalletsDto(){ DIDOrWalletID = "QmTLxy3Apc6XBc4mNvSqUpXAsoPA4LeA36SUXpkfpq3Zze", Balance = 64160},
                new TopWalletsDto(){ DIDOrWalletID = "QmdCcx7ToHQfFfCnUk6Sj667PEnKG89DMEyBUsrbbhZnpV", Balance = 64080},
                new TopWalletsDto(){ DIDOrWalletID = "QmdxvQD2J2Zt3pimFA3rhgGEinobh9STFFQbqy9SFrFoh6", Balance = 64004},
                new TopWalletsDto(){ DIDOrWalletID = "QmfF5RP1dsHhMe91bGjfnmUFMVEHKjS2UFfjjcjxXLCPE9", Balance = 63840},
                new TopWalletsDto(){ DIDOrWalletID = "QmPEgCLLWgi2QJd7iWSN37P8v4bsf4XZtyBM6yxmknSaiV", Balance = 63764},
                new TopWalletsDto(){ DIDOrWalletID = "QmU8CuyK6T6wRAEBm9a3hWs851GfjDhNwZBSxMRaJJ2K6M", Balance = 63524},
                new TopWalletsDto(){ DIDOrWalletID = "Qmdt8EeFrmEwYj5H66p5okTfVgMJmSmotfbzE9XzKj8eb2", Balance = 63520},
                new TopWalletsDto(){ DIDOrWalletID = "Qmb7WcvT7cfvfbfcJ6zvUMnhTsqhjmQRfkYKkLG9fZyiPN", Balance = 63204},
                new TopWalletsDto(){ DIDOrWalletID = "QmP41ixkXf41BBVpQVUd4KD2cYw2ZBEvhCfAZhUB2eFAdK", Balance = 63124},
                new TopWalletsDto(){ DIDOrWalletID = "QmVSj6TttaFg2fcBNXZdedjw7xur1jzZen3TzYZzeGSf8Z", Balance = 63120},
                new TopWalletsDto(){ DIDOrWalletID = "QmNeCzw69EEw1Rx6iaqQaWC8sWARV2bS2DYSzCWvhH3FHp", Balance = 63044},
                new TopWalletsDto(){ DIDOrWalletID = "QmTr3AzU8FNxDta29AnAjgxbeXGKGdJWbU2TZ7C6qiqyfW", Balance = 63040},
                new TopWalletsDto(){ DIDOrWalletID = "QmTDNPJQLgc9mBZJJJ32HX4wrbwmBYDGB2TkFFebK5kCGc", Balance = 62761},
                new TopWalletsDto(){ DIDOrWalletID = "QmZCHF2xzx6RY8NHCPFcbSRrmKbY57RbPoDNXUPxczpjys", Balance = 62724},
                new TopWalletsDto(){ DIDOrWalletID = "QmWZKjTjL1wmAxhcJuG8as16afSy2nMCBJZhaVjaAp91eX", Balance = 62720},
                new TopWalletsDto(){ DIDOrWalletID = "QmNeUVBafiqn7V9kiQEipw1z1eeVYNkfTeXwLK4kvd14Mk", Balance = 62403},
                new TopWalletsDto(){ DIDOrWalletID = "QmQcqjYBWdoABMSxziMxhyxBsWxj17gwExkzMD5qbNaWMv", Balance = 62401},
                new TopWalletsDto(){ DIDOrWalletID = "QmXxhAQPXJgZrgJDtvTs86NQLmRdWaGQn7bcPKyGX3Yktu", Balance = 62363},
                new TopWalletsDto(){ DIDOrWalletID = "QmUi5iBw37LJhxqjNLNuyr69vncjp9cvjyJeUd4VypACF6", Balance = 60724},
                new TopWalletsDto(){ DIDOrWalletID = "QmZ3jd2EiKAXnRnr8BV3BUHRqcHyDfGozQ3NgLpYgaMHBr", Balance = 59120},
                new TopWalletsDto(){ DIDOrWalletID = "QmR7Kyy6nQXYyiM3s3FtZKdeiVwJxLtXTUbdSJT3Pyi3J1", Balance = 57677},

                };

                return new PageResultDto<TopWalletsDto>
                {
                    Items = topWalletList
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<RubixTransaction>> GetSenderTransactionListByDIDAsync(string did)
        {
            var list = Collection.AsQueryable().Where(x => x.Sender_did == did).ToList();
            return list;
        }

        public async Task<List<RubixTransaction>> GetReciverTransactionListByDIDAsync(string did)
        {
            var list = Collection.AsQueryable().Where(x => x.Receiver_did == did).ToList();
            return list;
        }

        public async Task<double> GetTransactionalBalance(string user_did)
        {
            var senderFilter = Builders<RubixTransaction>.Filter.Eq("sender_did", user_did);
            var receiverFilter = Builders<RubixTransaction>.Filter.Eq("receiver_did", user_did);

            var senderTransactions = Collection.AsQueryable().Where(x => x.Sender_did == user_did).Select(x=>x.Amount).Sum();
            var receiverTransactions = Collection.AsQueryable().Where(x => x.Receiver_did == user_did).Select(x => x.Amount).Sum();

            double totalAmount= receiverTransactions- senderTransactions;

            return totalAmount;
        }

        public async Task<List<RubixTransaction>> GetTransactionsListByDaysAsync(long transactionsDays)
        {
            var pastDate = DateTime.Today.AddDays(-transactionsDays);
            var filter = Builders<RubixTransaction>.Filter.Gt(x=>x.CreationTime, pastDate.Date);
            var totalList = await Collection.Find(filter).SortByDescending(x => x.CreationTime).ToListAsync();
            return totalList;
        }
    }
}
