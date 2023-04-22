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

        private readonly IRepositoryRubixTransaction _repositoryRubixTransaction;


        private readonly IClientSessionHandle _clientSessionHandle;

        private readonly ILogger<RubixController> _logger;

        private readonly IRepositoryRubixTransactionQuorum _repositoryRubixTransactionQuorum;

        private readonly IRepositoryNFTTokenInfo _repositoryNFTTokenInfo;

        private readonly IDIDMapperRepository _dIDMapperRepository;

        private readonly IRepositoryRubixDataToken _repositoryRubixDataToken;

        private readonly IRepositoryCardsDashboard _repositoryCardsDashboard;

        public RubixV3Contorller(ILogger<RubixController> logger, IRepositoryRubixUser repositoryUser, IRepositoryRubixToken repositoryRubixToken, IRepositoryRubixTokenTransaction repositoryRubixTokenTransaction, IRepositoryRubixTransaction repositoryRubixTransaction, IClientSessionHandle clientSessionHandle, IRepositoryRubixTransactionQuorum repositoryRubixTransactionQuorum, IRepositoryNFTTokenInfo repositoryNFTTokenInfo, IDIDMapperRepository dIDMapperRepository, IRepositoryRubixDataToken repositoryRubixDataToken, IRepositoryCardsDashboard repositoryCardsDashboard) =>
            (_logger, _repositoryUser, _repositoryRubixToken, _repositoryRubixTokenTransaction, _repositoryRubixTransaction, _clientSessionHandle, _repositoryRubixTransactionQuorum, _repositoryNFTTokenInfo, _dIDMapperRepository, _repositoryRubixDataToken, _repositoryCardsDashboard) = (logger, repositoryUser, repositoryRubixToken, repositoryRubixTokenTransaction, repositoryRubixTransaction, clientSessionHandle, repositoryRubixTransactionQuorum, repositoryNFTTokenInfo, dIDMapperRepository, repositoryRubixDataToken, repositoryCardsDashboard);


        [HttpPost]
        [Route("create-nft-transaction")]
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
    }
}
