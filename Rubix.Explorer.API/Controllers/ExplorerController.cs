﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rubix.API.Shared.Enums;
using Rubix.API.Shared.Interfaces;
using Rubix.Explorer.API.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Rubix.API.Shared;
using Newtonsoft.Json;
using Rubix.API.Shared.Common;
using Rubix.API.Shared.Dto;
using Rubix.API.Shared.Entities;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Rubix.Explorer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExplorerController : ControllerBase
    {

        private readonly IRepositoryRubixUser _repositoryUser;

        private readonly IRepositoryRubixToken _repositoryRubixToken;

        private readonly IRepositoryRubixTokenTransaction _repositoryRubixTokenTransaction;

        private readonly IRepositoryRubixTransaction _repositoryRubixTransaction;


        private readonly IRepositoryDashboard _repositoryDashboard;

        private readonly IRepositoryCardsDashboard _repositoryCardsDashboard;

        private readonly IClientSessionHandle _clientSessionHandle;

        private readonly IMemoryCache _cache;

        public ExplorerController(IRepositoryRubixUser repositoryUser, IRepositoryRubixToken repositoryRubixToken, IRepositoryRubixTokenTransaction repositoryRubixTokenTransaction, IRepositoryRubixTransaction repositoryRubixTransaction, IClientSessionHandle clientSessionHandle, IRepositoryDashboard repositoryDashboard, IRepositoryCardsDashboard repositoryCardsDashboard, IMemoryCache cache) =>
            (_repositoryUser, _repositoryRubixToken, _repositoryRubixTokenTransaction, _repositoryRubixTransaction, _clientSessionHandle,_repositoryDashboard,_repositoryCardsDashboard,_cache) = (repositoryUser, repositoryRubixToken, repositoryRubixTokenTransaction, repositoryRubixTransaction, clientSessionHandle, repositoryDashboard,repositoryCardsDashboard, cache);

       

        [HttpGet]
        [Route("Cards")]
        public async Task<IActionResult> GetCardsData([FromQuery]ActivityFilter input)
        {
            try
            {
                if (!_cache.TryGetValue("cardsData", out RubixAnalyticsDto output))
                {
                    var data = await _repositoryCardsDashboard.FindByAsync();
                    if (data != null)
                    {
                        var obj = JsonConvert.DeserializeObject<CardsDto>(data.Data);

                        output = new RubixAnalyticsDto
                        {
                            RubixPrice = 0,
                            TransactionsCount = obj.TransCount,
                            TokensCount = obj.TokensCount,
                            RubixUsersCount = obj.UsersCount,
                            CurculatingSupplyCount = obj.CirculatingSupply
                        };
                        MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                        {
                            Priority = CacheItemPriority.High,
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60), // cache will expire in 5 mintues
                        };
                        _cache.Set("cardsData", output, options);
                        return StatusCode(StatusCodes.Status200OK, output);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new RubixAnalyticsDto());
                    }
                }
                return StatusCode(StatusCodes.Status200OK, output);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("LatestTokens")]
        public async Task<IActionResult> GetLatestTokens([FromQuery] GetAllTokensInput input)
        {
            try
            {
                var latestTokens = await _repositoryRubixToken.GetPagerResultAsync(input.Page, input.PageSize);
                return StatusCode(StatusCodes.Status200OK, latestTokens);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("LatestTransactions")]
        public async Task<IActionResult> GetLatestTransactions([FromQuery]GetAllTransactionsInput input)
        {
            try
            {
                var latestTransactions = await _repositoryRubixTransaction.GetPagedResultAsync(input.Page,input.PageSize);
                return StatusCode(StatusCodes.Status200OK, latestTransactions);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("DateWiseTransactions")]
        public async Task<IActionResult> GetDateWiseTransactions([FromQuery] ActivityFilter input)
        {
            try
            {
                var record = await _repositoryDashboard.FindByAsync(input,EntityType.Transactions);
                if(record!=null)
                {
                    var data = JsonConvert.DeserializeObject<List<Resultdto>>(record.Data);
                    return StatusCode(StatusCodes.Status200OK, data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }
              
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("DateWiseTokens")]
        public async Task<IActionResult> GetDateWiseTokens([FromQuery] ActivityFilter input)
        {
            try
            {
                var record = await _repositoryDashboard.FindByAsync(input, EntityType.Tokens);
                if (record != null)
                {
                    var data = JsonConvert.DeserializeObject<List<Resultdto>>(record.Data);
                    return StatusCode(StatusCodes.Status200OK, data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("userInfo/{user_did}")]
        public async Task<IActionResult> GetUserInfo([FromRoute]string user_did)
        {
            try
            {
                var res = await _repositoryUser.GetUserByUser_DIDAsync(user_did);
                if (res != null)
                {
                    var obj= new UserInfoDto { user_did = res.User_did, peerid = res.Peerid, ipaddress = res.IPaddress, balance = res.Balance };
                    return StatusCode(StatusCodes.Status200OK, obj);
                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("transactionInfo/{transaction_id}")]
        public async Task<IActionResult> GetTransactionInfo([FromRoute] string transaction_id)
        {

            try
            {
                var transData = await _repositoryRubixTransaction.FindByTransIdAsync(transaction_id);
                if(transData!=null)
                {
                    var token_id = await _repositoryRubixTokenTransaction.FindByTransIdAsync(transaction_id);
                    var obj = new TransactionInfoDto
                    {
                        transaction_id = transData.Transaction_id,
                        sender_did = transData.Sender_did,
                        receiver_did = transData.Receiver_did,
                        token = token_id.Token_id,
                        creationTime=transData.CreationTime,
                        amount=transData.Amount
                    };
                    return StatusCode(StatusCodes.Status200OK, obj);
                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent, new TransactionInfoDto());
                }
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("transactionListInfoForTokenId")]
        public async Task<IActionResult> GetTransactionsListInfo([FromQuery] GetAllTransactionsForTokensInput input)
        {


            List<TransactionDto> transactionList = new List<TransactionDto>();
            try
            {
                var transIds = await _repositoryRubixTokenTransaction.FindByTransByTokenIdAsync(input.Token_Id,input.PageSize,input.Page);

                foreach (var item in transIds.Items)
                {
                    var transIdData = await _repositoryRubixTransaction.FindByTransIdAsync(item.Transaction_id);
                    var data = new TransactionDto
                    {
                        amount = transIdData.Amount,
                        token_time = Math.Round((transIdData.Token_time / transIdData.Amount) / 1000, 3),
                        receiver_did = transIdData.Receiver_did,
                        transaction_id = transIdData.Transaction_id,
                        sender_did = transIdData.Sender_did,
                        time = transIdData.Token_time,
                        transaction_fee = 0
                    };
                    transactionList.Add(data);
                }
                var pageResult = new PageResultDto<TransactionDto>
                {
                    Count = transIds.Count,
                    Size = transIds.Size,
                    Page = transIds.Page,
                    Items = transactionList
                };
                return StatusCode(StatusCodes.Status200OK,pageResult);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetRBTDetails")]
        public async Task<IActionResult> GetRBTDetails()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.vindax.com/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync("api/v1/ticker/24hr?symbol=RBTUSDT");
                    var dataString = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(dataString))
                    {
                        var dataObj = JsonConvert.DeserializeObject<VindaxRBTDetailsDto>(dataString);
                        return StatusCode(StatusCodes.Status200OK, dataObj);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status204NoContent);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("getTransactions/{did}/{page}/{pageSize}")]
        public async Task<IActionResult> GetTransactionsByDID(string did,int page,int pageSize)
        {
            try
            {
                var obj = await _repositoryRubixTransaction.GetPagedResultByDIDAsync(did, page, pageSize);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
