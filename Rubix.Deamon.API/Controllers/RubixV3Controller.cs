using Microsoft.AspNetCore.Http;
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
using Rubix.API.Shared.Repositories;
using System.Linq.Expressions;

namespace Rubix.Deamon.API.Controllers
{
    [ApiVersion("3.0")]
    [Route("api/v{version:apiVersion}/services/app/Rubix")]
    [ApiController]
    public class RubixV3Contorller : ControllerBase 
    {
        private readonly IRepositoryRubixUser _repositoryUser;

        private readonly IRepositoryRubixToken _repositoryRubixToken;

        private readonly IRepositoryRubixTokenTransaction _repositoryRubixTokenTransaction;

        private readonly IRepositoryRubixNFTTransaction _repositoryRubixNFTTransaction; 


        private readonly IClientSessionHandle _clientSessionHandle;

        private readonly ILogger<RubixController> _logger;

        private readonly IRepositoryRubixTransactionQuorum _repositoryRubixTransactionQuorum;

        private readonly IRepositoryNFTTokenInfo _repositoryNFTTokenInfo;

        private readonly IDIDMapperRepository _dIDMapperRepository;

        private readonly IRepositoryRubixDataToken _repositoryRubixDataToken;

        private readonly IRepositoryCardsDashboard _repositoryCardsDashboard;

        private readonly IRepositoryRubixTransaction _repositoryRubixTransaction;



        public RubixV3Contorller(ILogger<RubixController> logger, IRepositoryRubixUser repositoryUser, IRepositoryRubixToken repositoryRubixToken, IRepositoryRubixTokenTransaction repositoryRubixTokenTransaction, IRepositoryRubixNFTTransaction repositoryRubixNFTTransaction, IClientSessionHandle clientSessionHandle, IRepositoryRubixTransactionQuorum repositoryRubixTransactionQuorum, IRepositoryNFTTokenInfo repositoryNFTTokenInfo, IDIDMapperRepository dIDMapperRepository, IRepositoryRubixDataToken repositoryRubixDataToken, IRepositoryCardsDashboard repositoryCardsDashboard, IRepositoryRubixTransaction repositoryRubixTransaction) =>
            (_logger, _repositoryUser, _repositoryRubixToken, _repositoryRubixTokenTransaction, _repositoryRubixNFTTransaction, _clientSessionHandle, _repositoryRubixTransactionQuorum, _repositoryNFTTokenInfo, _dIDMapperRepository, _repositoryRubixDataToken, _repositoryCardsDashboard, _repositoryRubixTransaction) = (logger, repositoryUser, repositoryRubixToken, repositoryRubixTokenTransaction, repositoryRubixNFTTransaction, clientSessionHandle, repositoryRubixTransactionQuorum, repositoryNFTTokenInfo, dIDMapperRepository, repositoryRubixDataToken, repositoryCardsDashboard, repositoryRubixTransaction);




        [HttpPost]
        [Route("create-rbt-transcation")]
        public async Task<IActionResult> CreateRBTTransactionAsync([FromBody] CreateRubixRBTTransactionDto transInput)
        {
            _clientSessionHandle.StartTransaction();

            try
            {
                var transactionInfo = new RubixTransaction(transInput.transaction_id, transInput.sender_did, transInput.receiver_did, transInput.token_time, transInput.amount,TransactionType.RBT,null,null,null,null,0,0,null,null,null);
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
                }

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
        [Route("create-nft-transaction")]
        public async Task<IActionResult> CreateTransactionAsync([FromBody] CreateRubixNFTTransactionDto transInput)
        {
            _clientSessionHandle.StartTransaction();

            try
            {
                var transactionInfo = new RubixNFTTransaction(TransactionType.NFT, transInput.nftToken, transInput.nftBuyer, transInput.nftSeller, transInput.nftCreatorInput, transInput.totalSupply, transInput.editionNumber, transInput.rbt_transaction_id, transInput.userHash);
                await _repositoryRubixNFTTransaction.InsertAsync(transactionInfo);

                if (transInput.token_id != null && transInput.token_id.Count() > 0)
                {
                    List<RubixTokenTransaction> tokenTrans = new List<RubixTokenTransaction>();
                    foreach (var u in transInput.token_id)
                    {
                        var obj = new RubixTokenTransaction(transInput.rbt_transaction_id, u);
                        obj.CreationTime = DateTime.UtcNow;
                        tokenTrans.Add(obj);
                    }
                    await _repositoryRubixTokenTransaction.InsertManyAsync(tokenTrans);
                }

                // Sender
                var transactionSender = await _repositoryUser.GetUserByUser_DIDAsync(transInput.nftSeller);
                if (transactionSender != null)
                {
                    transactionSender.Balance -= 0;
                    await _repositoryUser.UpdateAsync(transactionSender);
                }

                //Reciver
                var transactionReceiver = await _repositoryUser.GetUserByUser_DIDAsync(transInput.nftBuyer);
                if (transactionReceiver != null)
                {
                    transactionReceiver.Balance += 0;
                    await _repositoryUser.UpdateAsync(transactionReceiver);
                }
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

        [HttpPut]
        [Route("set-node-status/{peerid}/{status}")]
        public async Task<IActionResult> SetNodeStatus(string peerid,bool status) 
        {
            _clientSessionHandle.StartTransaction();
            var output = new RubixCommonOutput();
            try
            {
                var peerInfo=await _repositoryUser.GetNodeByPeerIdAsync(peerid);
                if(peerInfo!=null)
                {
                    await _repositoryUser.UpdateAsync(new RubixUser(peerInfo.User_did, peerInfo.Peerid, peerInfo.IPaddress, peerInfo.Balance, status));
                    await _clientSessionHandle.CommitTransactionAsync();
                    output.Status = true;
                    output.Message = "Node Status Successfully Setted";
                }
                else
                {
                    output.Status = false;
                    output.Message = $"Node not found with this peerId: {peerid}";
                }
                return StatusCode(StatusCodes.Status200OK, output);
            }
            catch (MongoBulkWriteException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                output.Status = false;
                output.Message = ex.Message;
                return StatusCode(StatusCodes.Status406NotAcceptable, output);
            }
            catch (MongoWriteException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                output.Status = false;
                output.Message = ex.Message;
                return StatusCode(StatusCodes.Status406NotAcceptable, output);
            }
            catch (MongoAuthenticationException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                output.Status = false;
                output.Message = ex.Message;
                return StatusCode(StatusCodes.Status407ProxyAuthenticationRequired, output);
            }
            catch (MongoConnectionException ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                output.Status = false;
                output.Message = ex.Message;
                return StatusCode(StatusCodes.Status408RequestTimeout, output);
            }
            catch (Exception ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                output.Status = false;
                output.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, output);
            }
        }

    }
}
