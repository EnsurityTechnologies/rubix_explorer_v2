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

        public ExplorerController(IRepositoryRubixUser repositoryUser, IRepositoryRubixToken repositoryRubixToken, IRepositoryRubixTokenTransaction repositoryRubixTokenTransaction, IRepositoryRubixTransaction repositoryRubixTransaction, IClientSessionHandle clientSessionHandle, IRepositoryDashboard repositoryDashboard, IRepositoryCardsDashboard repositoryCardsDashboard) =>
            (_repositoryUser, _repositoryRubixToken, _repositoryRubixTokenTransaction, _repositoryRubixTransaction, _clientSessionHandle,_repositoryDashboard,_repositoryCardsDashboard) = (repositoryUser, repositoryRubixToken, repositoryRubixTokenTransaction, repositoryRubixTransaction, clientSessionHandle, repositoryDashboard,repositoryCardsDashboard);

       

        [HttpGet]
        [Route("Cards")]
        public async Task<IActionResult> GetCardsData([FromQuery]ActivityFilter input)
        {
            try
            {

                var data = await _repositoryCardsDashboard.FindByAsync(input);

                if(data!=null)
                {
                    var obj = JsonConvert.DeserializeObject<CardsDto>(data.Data);

                    var output = new RubixAnalyticsDto
                    {
                        RubixPrice = 0,
                        TransactionsCount = obj.TransCount,
                        TokensCount = obj.TokensCount,
                        RubixUsersCount = obj.UsersCount,
                        CurculatingSupplyCount=obj.CirculatingSupply
                       
                    };
                    return StatusCode(StatusCodes.Status200OK, output);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new RubixAnalyticsDto());
                }
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
                int i = 1;
                var transData = await _repositoryRubixTransaction.GetAllAsync();
                var res = from a in await _repositoryRubixTransaction.GetAllAsync()
                          join b in await _repositoryRubixTokenTransaction.GetAllAsync() on a.Transaction_id equals b.Transaction_id
                          where a.Transaction_id == transaction_id
                          select new TransactionInfoDto
                          {
                              transaction_id = a.Transaction_id,
                              sender_did = a.Sender_did,
                              receiver_did = a.Receiver_did,
                              token = b.Token_id
                          };
                var totalRes = from a in res.ToList()
                               select new TransactionInfoDto
                               {
                                   Id = i++,
                                   sender_did = a.sender_did,
                                   receiver_did = a.receiver_did,
                                   transaction_id = a.transaction_id,
                                   token = a.token
                               };
                var transactions = totalRes.ToList();
                return StatusCode(StatusCodes.Status200OK, transactions);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return StatusCode(StatusCodes.Status200OK); 
        }
    }
}
