﻿using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Interfaces;
using Rubix.Deamon.API.Models;
using Rubix.Deamon.API.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.Deamon.API.Controllers
{
    [Route("api/services/app/Rubix")]
    [ApiController]
    public class RubixController : ControllerBase 
    {
        private readonly IRepositoryRubixUser _repositoryUser;

        private readonly IRepositoryRubixToken _repositoryRubixToken;

        private readonly IRepositoryRubixTokenTransaction _repositoryRubixTokenTransaction;

        private readonly IRepositoryRubixTransaction _repositoryRubixTransaction; 


        private readonly IClientSessionHandle _clientSessionHandle;

        private readonly ILogger<RubixController> _logger;

        public RubixController(ILogger<RubixController> logger,IRepositoryRubixUser repositoryUser, IRepositoryRubixToken repositoryRubixToken, IRepositoryRubixTokenTransaction repositoryRubixTokenTransaction, IRepositoryRubixTransaction repositoryRubixTransaction,IClientSessionHandle clientSessionHandle) =>
            (_logger, _repositoryUser, _repositoryRubixToken, _repositoryRubixTokenTransaction, _repositoryRubixTransaction, _clientSessionHandle) = (logger,repositoryUser, repositoryRubixToken, repositoryRubixTokenTransaction, repositoryRubixTransaction, clientSessionHandle);


        [HttpPost]
        [Route("CreateOrUpdateRubixUser")]
        public async Task<IActionResult> CreateUserAsync([FromBody] RubixCommonInput input)
        {
             var userInput = JsonConvert.DeserializeObject<CreateRubixUserDto>(input.InputString);

            _clientSessionHandle.StartTransaction();

            try
            {
                _logger.LogInformation("request from CreateOrUpdateRubixUser Serilized: input:{0}", input.InputString);

                await _repositoryUser.InsertAsync(new RubixUser(userInput.user_did, userInput.peerid, userInput.ipaddress, userInput.balance));

                await _clientSessionHandle.CommitTransactionAsync();

                var output= new RubixCommonOutput { Status = true, Message = "User created sucessfully" };

                return StatusCode(StatusCodes.Status200OK, output);
            }
            catch (Exception ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };

                _logger.LogError("Error CreateOrUpdateRubixUser Exception:{0}", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, output);
            }
        }

        [HttpPost]
        [Route("CreateOrUpdateRubixTransaction")]
        public async Task<IActionResult> CreateTransactionAsync([FromBody] RubixCommonInput input)
        {
             var transInput = JsonConvert.DeserializeObject<CreateRubixTransactionDto>(input.InputString);
            _clientSessionHandle.StartTransaction();

            try
            {
                _logger.LogInformation("request from CreateOrUpdateRubixTransaction Serilized: input:{0}", input.InputString);

                var transactionInfo = new RubixTransaction(transInput.transaction_id, transInput.sender_did, transInput.receiver_did,  transInput.token_time, transInput.amount);
                await _repositoryRubixTransaction.InsertAsync(transactionInfo);

                List<RubixTokenTransaction> tokenTrans = new List<RubixTokenTransaction>();
                foreach (var u in transInput.token_id)
                {
                    var obj = new RubixTokenTransaction(transInput.transaction_id, u);
                    obj.CreationTime = DateTime.UtcNow;
                    tokenTrans.Add(obj); 
                }
                await _repositoryRubixTokenTransaction.InsertManyAsync(tokenTrans);

                // Sender
                var transactionSender = await _repositoryUser.GetUserByUser_DIDAsync(transInput.sender_did);
                if (transactionSender != null)
                {
                    transactionSender.Balance -= transInput.amount;
                    await _repositoryUser.UpdateAsync(transactionSender);
                }

                //Reciver
                var transactionReceiver = await _repositoryUser.GetUserByUser_DIDAsync(transInput.sender_did);
                if (transactionReceiver != null)
                {
                    transactionReceiver.Balance += transInput.amount;
                    await _repositoryUser.UpdateAsync(transactionReceiver);
                }

                var output= new RubixCommonOutput { Status = true, Message = "Transaction created sucessfully" };

                await _clientSessionHandle.CommitTransactionAsync();

                return StatusCode(StatusCodes.Status200OK, output);
            }
            catch (Exception ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message =ex.Message};

                _logger.LogError("Error CreateOrUpdateRubixTransaction Exception:{0}", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, output);
            }
        }

        [HttpPost]
        [Route("CreateOrUpdateRubixToken")]
        public async Task<IActionResult> CreatTokenAsync([FromBody] RubixCommonInput input) 
        {
             var tokenIput = JsonConvert.DeserializeObject<CreateRubixTokenDto>(input.InputString);
            _clientSessionHandle.StartTransaction();
            try
            {

                _logger.LogInformation("request from CreateOrUpdateRubixToken Serilized: input:{0}", input.InputString);

                List<RubixToken> tokens = new List<RubixToken>();
                foreach (var u in tokenIput.token_id)
                {
                    var obj = new RubixToken(u, tokenIput.bank_id, tokenIput.denomination, tokenIput.user_did, tokenIput.level);
                    obj.CreationTime = DateTime.UtcNow;
                    tokens.Add(obj);
                }
                _logger.LogInformation("request from CreateOrUpdateRubixToken tokens count:{0}", tokens.Count());

                await _repositoryRubixToken.InsertManyAsync(tokens);

                var tokenUser = await _repositoryUser.GetUserByUser_DIDAsync(tokenIput.user_did);
                if (tokenUser != null)
                {
                    tokenUser.Balance = 1;
                    await _repositoryUser.UpdateAsync(tokenUser);
                }
                var output= new RubixCommonOutput { Status = true, Message = "Token created sucessfully" };
                await _clientSessionHandle.CommitTransactionAsync();
                return StatusCode(StatusCodes.Status200OK, output);
            }
            catch (Exception ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message =ex.Message};

                _logger.LogError("Error CreateOrUpdateRubixToken Exception:{0}", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, output);
            }
        }


        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> TestLog()
        {
            try
            {
                _logger.LogInformation("request from TestLog");
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error CreateOrUpdateRubixToken Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
