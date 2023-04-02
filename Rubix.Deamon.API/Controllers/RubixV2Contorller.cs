﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Interfaces;
using Rubix.Deamon.API.Models.Dto;
using Rubix.Deamon.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace Rubix.Deamon.API.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/services/app/Rubix")]
    [ApiController]
    public class RubixV2Contorller : ControllerBase
    {
        private readonly IRepositoryRubixUser _repositoryUser;

        private readonly IRepositoryRubixToken _repositoryRubixToken;

        private readonly IRepositoryRubixTokenTransaction _repositoryRubixTokenTransaction;

        private readonly IRepositoryRubixTransaction _repositoryRubixTransaction;


        private readonly IClientSessionHandle _clientSessionHandle;

        private readonly ILogger<RubixController> _logger;

        private readonly IRepositoryRubixTransactionQuorum _repositoryRubixTransactionQuorum;

        private readonly IRepositoryNFTTokenInfo _repositoryNFTTokenInfo;

        private readonly IDIDMapperRepository _dIDMapperRepository;

        private readonly IRepositoryRubixDataToken _repositoryRubixDataToken;

        public RubixV2Contorller(ILogger<RubixController> logger, IRepositoryRubixUser repositoryUser, IRepositoryRubixToken repositoryRubixToken, IRepositoryRubixTokenTransaction repositoryRubixTokenTransaction, IRepositoryRubixTransaction repositoryRubixTransaction, IClientSessionHandle clientSessionHandle, IRepositoryRubixTransactionQuorum repositoryRubixTransactionQuorum, IRepositoryNFTTokenInfo repositoryNFTTokenInfo, IDIDMapperRepository dIDMapperRepository, IRepositoryRubixDataToken repositoryRubixDataToken) =>
            (_logger, _repositoryUser, _repositoryRubixToken, _repositoryRubixTokenTransaction, _repositoryRubixTransaction, _clientSessionHandle, _repositoryRubixTransactionQuorum, _repositoryNFTTokenInfo, _dIDMapperRepository,_repositoryRubixDataToken) = (logger, repositoryUser, repositoryRubixToken, repositoryRubixTokenTransaction, repositoryRubixTransaction, clientSessionHandle, repositoryRubixTransactionQuorum, repositoryNFTTokenInfo, dIDMapperRepository,repositoryRubixDataToken);


        [HttpPost]
        [Route("CreateOrUpdateRubixUser")]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateRubixUserDto userInput)
        {
            _clientSessionHandle.StartTransaction();

            try
            {
                await _repositoryUser.InsertAsync(new RubixUser(userInput.user_did, userInput.peerid, userInput.ipaddress, userInput.balance));

                await _clientSessionHandle.CommitTransactionAsync();

                var output = new RubixCommonOutput { Status = true, Message = "User created sucessfully" };

                return StatusCode(StatusCodes.Status200OK, output);
            }
            catch (MongoBulkWriteException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixUser Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status406NotAcceptable, output);
            }
            catch (MongoWriteException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixUser Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status406NotAcceptable, output);
            }
            catch (MongoAuthenticationException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixUser Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status407ProxyAuthenticationRequired, output);
            }
            catch (MongoConnectionException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixUser Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status408RequestTimeout, output);
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
        public async Task<IActionResult> CreateTransactionAsync([FromBody] CreateRubixTransactionDto transInput)
        {
            _clientSessionHandle.StartTransaction();

            try
            {
                var transactionInfo = new RubixTransaction(transInput.transaction_id, transInput.sender_did, transInput.receiver_did, transInput.token_time, transInput.amount, transInput.transaction_type, transInput.nftToken, transInput.nftBuyer, transInput.nftSeller, transInput.nftCreatorInput, transInput.totalSupply, transInput.editionNumber, transInput.rbt_transaction_id, transInput.userHash, transInput.block_hash);
                await _repositoryRubixTransaction.InsertAsync(transactionInfo);

                if (transInput.token_id != null && transInput.token_id.Count() > 0)
                {
                    List<RubixTokenTransaction> tokenTrans = new List<RubixTokenTransaction>();
                    foreach (var u in transInput.token_id)
                    {
                        var obj = new RubixTokenTransaction(transInput.transaction_id, u);
                        obj.CreationTime = DateTime.UtcNow;
                        tokenTrans.Add(obj);
                    }
                    await _repositoryRubixTokenTransaction.InsertManyAsync(tokenTrans);
                }


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

                    //Send Email..
                    //var sendEmail = await SendEmail(new SendEmailRequest()
                    //{
                    //    SenderDiD = transactionSender.User_did,
                    //    RecieverDiD = transactionReceiver.User_did,
                    //    TransferedBalance = transactionReceiver.Balance,

                    //});
                }

                // Adding Transaction Quorum List
                //if(transInput.quorum_list.Count > 0)
                //{
                //    var quorum_list = JsonConvert.SerializeObject(transInput.quorum_list);
                //    await _repositoryRubixTransactionQuorum.InsertAsync(new RubixTransactionQuorum(transInput.transaction_id, quorum_list));
                //}
                //else
                //{
                //   // await _clientSessionHandle.CommitTransactionAsync();
                //    //return StatusCode(StatusCodes.Status206PartialContent, new RubixCommonOutput { Status = true, Message = String.Format("Quorum List not received for this transaction: {0}",transInput.transaction_id) });
                //}

                var output = new RubixCommonOutput { Status = true, Message = "Transaction created sucessfully" };

                await _clientSessionHandle.CommitTransactionAsync();

                return StatusCode(StatusCodes.Status200OK, output);
            }
            catch (MongoBulkWriteException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixTransaction Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status406NotAcceptable, output);
            }
            catch (MongoWriteException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixTransaction Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status406NotAcceptable, output);
            }
            catch (MongoAuthenticationException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixTransaction Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status407ProxyAuthenticationRequired, output);
            }
            catch (MongoConnectionException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixTransaction Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status408RequestTimeout, output);
            }
            catch (Exception ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };

                _logger.LogError("Error CreateOrUpdateRubixTransaction Exception:{0}", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, output);
            }
        }



        [HttpPost]
        [Route("CreateOrUpdateRubixToken")]
        public async Task<IActionResult> CreatTokenAsync([FromBody] CreateRubixTokenDto tokenIput)
        {
            _clientSessionHandle.StartTransaction();
            try
            {
                List<RubixToken> tokens = new List<RubixToken>();
                foreach (var u in tokenIput.token_id)
                {
                    var obj = new RubixToken(u, tokenIput.bank_id, tokenIput.denomination, tokenIput.user_did, tokenIput.level);
                    obj.CreationTime = DateTime.UtcNow;
                    tokens.Add(obj);
                }

                await _repositoryRubixToken.InsertManyAsync(tokens);

                var tokenUser = await _repositoryUser.GetUserByUser_DIDAsync(tokenIput.user_did);
                if (tokenUser != null)
                {
                    tokenUser.Balance = 1;
                    await _repositoryUser.UpdateAsync(tokenUser);
                }
                var output = new RubixCommonOutput { Status = true, Message = "Token created sucessfully" };
                await _clientSessionHandle.CommitTransactionAsync();
                return StatusCode(StatusCodes.Status200OK, output);
            }
            catch (MongoBulkWriteException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixToken Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status406NotAcceptable, output);
            }
            catch (MongoWriteException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixToken Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status406NotAcceptable, output);
            }
            catch (MongoAuthenticationException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixToken Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status407ProxyAuthenticationRequired, output);
            }
            catch (MongoConnectionException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixToken Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status408RequestTimeout, output);
            }
            catch (Exception ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixToken Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, output);
            }
        }



        [HttpPost]
        [Route("map-did")]
        public async Task<IActionResult> MAPDIDAsync([FromBody] MapDIDRequest didInfo)
        {
            _clientSessionHandle.StartTransaction();
            try
            {
                await _dIDMapperRepository.InsertAsync(new DIDMapper(didInfo.new_did, didInfo.old_did,didInfo.peer_id,DateTime.UtcNow));
                var output = new RubixCommonOutput { Status = true, Message = "Token created sucessfully" };
                await _clientSessionHandle.CommitTransactionAsync();
                return StatusCode(StatusCodes.Status200OK, output);
            }
            catch (MongoBulkWriteException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixToken Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status406NotAcceptable, output);
            }
            catch (MongoWriteException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixToken Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status406NotAcceptable, output);
            }
            catch (MongoAuthenticationException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixToken Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status407ProxyAuthenticationRequired, output);
            }
            catch (MongoConnectionException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixToken Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status408RequestTimeout, output);
            }
            catch (Exception ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixToken Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, output);
            }
        }

        [HttpGet]
        [Route("check-mined-status/{tokenhash}")]
        public async Task<IActionResult> IsTokenMined(string tokenhash)
        {
            try
            {
                if (tokenhash.Length == 67)
                {
                    var response = await _repositoryRubixToken.IsMinedToken(tokenhash);
                    if (response)
                    {
                        return StatusCode(StatusCodes.Status200OK, new
                        {
                            status = response,
                            message = "token mined"
                        });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new
                        {
                            status = response,
                            message = "token not yet mined"
                        });
                    }

                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        status = false,
                        message = "bad request, invalid input"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = ex.Message
                });
            }
        }


        [HttpPost]
        [Route("newMint")]
        public async Task<IActionResult> MintToken([FromBody] CreateNFTTokenInput tokenIput)
        {
            try
            {
                _clientSessionHandle.StartTransaction();
                try
                {

                    var nftTokkenInfo = new NFTTokenInfo(tokenIput.tokenType, tokenIput.creatorId, tokenIput.nftToken, tokenIput.creatorPubKeyIpfsHash, tokenIput.totalSupply, tokenIput.edition, tokenIput.url, tokenIput.createdOn);
                    nftTokkenInfo.CreationTime = DateTime.UtcNow;
                    await _repositoryNFTTokenInfo.InsertAsync(nftTokkenInfo);

                    var output = new RubixCommonOutput { Status = true, Message = "Token Minted sucessfully" };
                    await _clientSessionHandle.CommitTransactionAsync();
                    return StatusCode(StatusCodes.Status200OK, output);
                }
                catch (MongoBulkWriteException ex)
                {
                    await _clientSessionHandle.AbortTransactionAsync();
                    var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                    _logger.LogError("Error MintToken Exception:{0}", ex.Message);
                    return StatusCode(StatusCodes.Status406NotAcceptable, output);
                }
                catch (MongoWriteException ex)
                {
                    await _clientSessionHandle.AbortTransactionAsync();
                    var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                    _logger.LogError("Error MintToken Exception:{0}", ex.Message);
                    return StatusCode(StatusCodes.Status406NotAcceptable, output);
                }
                catch (MongoAuthenticationException ex)
                {
                    await _clientSessionHandle.AbortTransactionAsync();
                    var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                    _logger.LogError("Error MintToken Exception:{0}", ex.Message);
                    return StatusCode(StatusCodes.Status407ProxyAuthenticationRequired, output);
                }
                catch (MongoConnectionException ex)
                {
                    await _clientSessionHandle.AbortTransactionAsync();
                    var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                    _logger.LogError("Error MintToken Exception:{0}", ex.Message);
                    return StatusCode(StatusCodes.Status408RequestTimeout, output);
                }
                catch (Exception ex)
                {
                    await _clientSessionHandle.AbortTransactionAsync();
                    var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                    _logger.LogError("Error MintToken Exception:{0}", ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, output);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [Route("create-datatoken")]
        public async Task<IActionResult> CreateDataTokenTransactionAsync([FromBody] CreateDataTokenDto transInput)
        {
            _clientSessionHandle.StartTransaction();

            try
            {
                var transactionInfo = new RubixTransaction(transInput.transaction_id, transInput.sender, transInput.receiver, transInput.time, transInput.amount, transInput.transaction_type,null,null,null,null,0,0, transInput.rbt_transaction_id,null,null);
                await _repositoryRubixTransaction.InsertAsync(transactionInfo);

                if (transInput.datatokens != null && transInput.datatokens.Count() > 0)
                {
                    List<RubixTokenTransaction> tokenTrans = new List<RubixTokenTransaction>();
                    foreach (var u in transInput.datatokens)
                    {
                        var obj = new RubixTokenTransaction(transInput.transaction_id,u.Value);
                        obj.CreationTime = DateTime.UtcNow;
                        tokenTrans.Add(obj);
                    }
                    await _repositoryRubixTokenTransaction.InsertManyAsync(tokenTrans);
                }



                await _repositoryRubixDataToken.InsertAsync(new RubixDataToken() { 
                    sender= transInput.sender,
                    amount= transInput.amount,
                    datatokens= transInput.datatokens,
                    commiter= transInput.commiter,
                    CreationTime=DateTime.UtcNow,
                    quorum_list= transInput.quorum_list,
                    rbt_transaction_id= transInput.rbt_transaction_id,
                    receiver= transInput.receiver,
                    time= transInput.time,
                    transaction_id= transInput.transaction_id,
                    transaction_type= transInput.transaction_type,
                });


                // Sender
                var transactionSender = await _repositoryUser.GetUserByUser_DIDAsync(transInput.sender);
                if (transactionSender != null)
                {
                    transactionSender.Balance -= transInput.amount;
                    await _repositoryUser.UpdateAsync(transactionSender);
                }

                //Reciver
                var transactionReceiver = await _repositoryUser.GetUserByUser_DIDAsync(transInput.receiver);
                if (transactionReceiver != null)
                {
                    transactionReceiver.Balance += transInput.amount;
                    await _repositoryUser.UpdateAsync(transactionReceiver);
                }

                //// Adding Transaction Quorum List
                //if (transInput.quorum_list.Count > 0)
                //{
                //    var quorum_list = JsonConvert.SerializeObject(transInput.quorum_list);
                //    await _repositoryRubixTransactionQuorum.InsertAsync(new RubixTransactionQuorum(transInput.transaction_id, quorum_list));
                //}
                //else
                //{
                //    // await _clientSessionHandle.CommitTransactionAsync();
                //    //return StatusCode(StatusCodes.Status206PartialContent, new RubixCommonOutput { Status = true, Message = String.Format("Quorum List not received for this transaction: {0}",transInput.transaction_id) });
                //}

                var output = new RubixCommonOutput { Status = true, Message = "Transaction created sucessfully" };

                await _clientSessionHandle.CommitTransactionAsync();

                return StatusCode(StatusCodes.Status200OK, output);
            }
            catch (MongoBulkWriteException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixTransaction Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status406NotAcceptable, output);
            }
            catch (MongoWriteException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixTransaction Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status406NotAcceptable, output);
            }
            catch (MongoAuthenticationException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixTransaction Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status407ProxyAuthenticationRequired, output);
            }
            catch (MongoConnectionException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };
                _logger.LogError("Error CreateOrUpdateRubixTransaction Exception:{0}", ex.Message);
                return StatusCode(StatusCodes.Status408RequestTimeout, output);
            }
            catch (Exception ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                var output = new RubixCommonOutput { Status = false, Message = ex.Message };

                _logger.LogError("Error CreateOrUpdateRubixTransaction Exception:{0}", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, output);
            }
        }

        [Route("test")]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }

    }
}