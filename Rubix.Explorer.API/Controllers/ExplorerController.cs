using Microsoft.AspNetCore.Http;
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

        private readonly ILevelBasedTokenRepository _levelBasedTokenRepository;

        private readonly IClientSessionHandle _clientSessionHandle;

        private readonly IRepositoryRubixTransactionQuorum _repositoryRubixTransactionQuorum;

        private readonly IRepositoryRubixDataToken _repositoryRubixDataToken;

        private readonly IMemoryCache _cache;

        private readonly IDIDMapperRepository _dIDMapperRepository;

        public ExplorerController(IRepositoryRubixUser repositoryUser, IRepositoryRubixToken repositoryRubixToken, IRepositoryRubixTokenTransaction repositoryRubixTokenTransaction, IRepositoryRubixTransaction repositoryRubixTransaction, ILevelBasedTokenRepository levelBasedTokenRepository, IClientSessionHandle clientSessionHandle, IRepositoryDashboard repositoryDashboard, IRepositoryCardsDashboard repositoryCardsDashboard, IMemoryCache cache, IRepositoryRubixTransactionQuorum repositoryRubixTransactionQuorum, IDIDMapperRepository dIDMapperRepository, IRepositoryRubixDataToken repositoryRubixDataToken) =>
            (_repositoryUser, _repositoryRubixToken, _repositoryRubixTokenTransaction, _repositoryRubixTransaction, _levelBasedTokenRepository, _clientSessionHandle, _repositoryDashboard, _repositoryCardsDashboard, _cache, _repositoryRubixTransactionQuorum, _dIDMapperRepository, _repositoryRubixDataToken) = (repositoryUser, repositoryRubixToken, repositoryRubixTokenTransaction, repositoryRubixTransaction, levelBasedTokenRepository, clientSessionHandle, repositoryDashboard, repositoryCardsDashboard, cache, repositoryRubixTransactionQuorum, dIDMapperRepository, repositoryRubixDataToken);



        [HttpGet]
        [Route("totalsupply")]
        public async Task<IActionResult> TotalSupply()
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { totalcoins = 51400000 });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



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
                        //var rbtInfo = await GetRBTInfoVindax();
                          var rbtInfo = await GetRBTInfoLBank();
                         //var rbtInfo=await getWhiteBITRBTInfo();
                        output = new RubixAnalyticsDto
                        {

                            //White Bit
                            //RubixPrice = rbtInfo.result == null ? 0.00 : Convert.ToDouble(rbtInfo.result.high),

                            //Vindax
                           // RubixPrice = rbtInfo.highPrice,
                           
                            //Lbank
                             RubixPrice = rbtInfo.data == null ? 135.00 : Convert.ToDouble(rbtInfo.data[0].ticker.high),
                           
                            
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
        [Route("get-latest-datatokens")]
        public async Task<IActionResult> GetLatestDataTokens([FromQuery] GetAllTokensInput input)
        {
            try
            {
                var latestTokens = await _repositoryRubixDataToken.GetPagedResultAsync(input.Page, input.PageSize);
                return StatusCode(StatusCodes.Status200OK, latestTokens);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("datatokenInfo/{transaction_id}")]
        public async Task<IActionResult> GetDatatokenInfo([FromRoute] string transaction_id)
        {

            try
            {
                var transData = await _repositoryRubixDataToken.FindByTransIdAsync(transaction_id);
                if (transData != null)
                {
                    var obj = new DatatokenDto
                    {
                        transaction_id = transData.transaction_id,
                        commiter = transData.commiter,
                        time = transData.time,
                        amount = transData.amount,
                        token_time = Math.Round((transData.time / transData.amount) / 1000, 3),
                        transaction_fee = 0,
                        creation_time = transData.CreationTime,
                        transaction_type = transData.transaction_type,
                        quorum_list = transData.quorum_list,
                        datatokens = transData.datatokens

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
        [Route("GetQuorumList/{transaction_id}")]
        public async Task<IActionResult> GetLatestTransactions(string transaction_id)
        {
            try
            {
                var quorumInfo = await _repositoryRubixTransactionQuorum.FindByTransIdAsync(transaction_id);
                if(quorumInfo == null)
                {
                    var transQuorumListObject = new
                    {
                        transaction_id ="",
                        quorum_list = new List<string>()
                    };
                    return StatusCode(StatusCodes.Status204NoContent, transQuorumListObject);
                }
                else
                {
                    var transQuorumListObject = new
                    {
                        transaction_id = quorumInfo.Transaction_id,
                        quorum_list = JsonConvert.DeserializeObject<List<string>>(quorumInfo.Quorum_List)
                    };
                  return StatusCode(StatusCodes.Status200OK, transQuorumListObject);
                }
                
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
                if(res==null)
                {
                    var old_DID =await _dIDMapperRepository.GetOldDIDInfo(user_did);
                    if(old_DID!=null)
                    {
                        var newresp = await _repositoryUser.GetUserByUser_DIDAsync(old_DID.OldDID);
                        if(newresp!=null)
                        {
                            var obj = new UserInfoDto { user_did = newresp.User_did, peerid = newresp.Peerid, ipaddress = newresp.IPaddress, balance = newresp.Balance,new_did= old_DID.NewDID,new_peerId= old_DID.PeerID};
                            return StatusCode(StatusCodes.Status200OK, obj);
                        }
                    }
                }
                if (res != null)
                {
                    var obj = new UserInfoDto { user_did = res.User_did, peerid = res.Peerid, ipaddress = res.IPaddress, balance = res.Balance };
                    var old_DID = await _dIDMapperRepository.GetNewDIDInfo(user_did);
                    if (old_DID != null)
                    {
                        obj.new_did = old_DID.NewDID;
                        obj.new_peerId = old_DID.PeerID;
                    }
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
                        token = token_id?.Token_id,
                        creationTime=transData.CreationTime,
                        amount=transData.Amount,
                        NftSeller=transData.NftSeller,
                        TotalSupply=transData.TotalSupply,
                        EditionNumber=transData.EditionNumber,
                        NftBuyer=transData.NftBuyer,
                        NftCreatorInput=transData.NftCreatorInput,
                        NftToken=transData.NftToken,
                        RBTTransactionId=transData.RBTTransactionId,
                        TransactionType= transData.TransactionType,
                        UserHash=transData.UserHash,
                        BlockHash=transData.BlockHash
                        
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
                        transaction_fee = 0,
                        NftSeller = transIdData.NftSeller,
                        TotalSupply = transIdData.TotalSupply,
                        EditionNumber = transIdData.EditionNumber,
                        NftBuyer = transIdData.NftBuyer,
                        NftCreatorInput = transIdData.NftCreatorInput,
                        NftToken = transIdData.NftToken,
                        RBTTransactionId = transIdData.RBTTransactionId,
                        TransactionType = transIdData.TransactionType,
                        UserHash = transIdData.UserHash,
                        BlockHash=transIdData.BlockHash
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
        [Route("levelBasedTokensCount")]
        public async Task<IActionResult> GetLevelBasedTokensCountAsync()
        {
            try
            {
               var levelBasedTokensData = _levelBasedTokenRepository.GetAllAsync().Result.Where(x=>x.LastModificationTime == DateTime.Today).FirstOrDefault();
                var levelsData = JsonConvert.DeserializeObject<List<LevelBasedTokensDto>>(levelBasedTokensData.Data);
                
                return StatusCode(StatusCodes.Status200OK, levelsData);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //[HttpGet]
        //[Route("levelBasedTokensCount")]
        //public async Task<IActionResult> GetLevelBasedTokensCountAsync()
        //{
        //    try
        //    {
        //        var rubixTokensList = _repositoryRubixToken.GetAllAsync().Result.Select(x=>x.Level);
        //        var groupedTokensList = (from rbxTokens in rubixTokensList
        //                                  group rbxTokens by rbxTokens into newTokensGroup
        //                                  select new
        //                                  {
        //                                      Level = newTokensGroup.Key,
        //                                      count = newTokensGroup.Count()
        //                                  });
        //        var Level1="Level 1";
        //        var LevelCount = 0;
        //        List<LevelBasedTokensDto> levelBasedTokens = new List<LevelBasedTokensDto>();
        //        foreach (var item in groupedTokensList)
        //        {
        //            if (item.Level == "01" || item.Level == "1" || item.Level == "Level1")
        //            {
        //                LevelCount = LevelCount + item.count;
        //            }
        //            else
        //            {
        //                levelBasedTokens.Add(new LevelBasedTokensDto()
        //                {
        //                    Level = "Level "+item.Level,
        //                    Count = item.count
        //                });
        //            }
        //        }
        //        var level1Record = new LevelBasedTokensDto() {Level = Level1, Count = LevelCount };
        //        levelBasedTokens.Add(level1Record);
        //        var totalLevelBasedData = levelBasedTokens.OrderBy(x=>x.Level);
        //        return StatusCode(StatusCodes.Status200OK, totalLevelBasedData);
        //    }
        //    catch (Exception ex)
        //    {

        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}

        private async Task<VindaxRBTDetailsDto> GetRBTInfoVindax()
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
                        return dataObj;
                    }
                    else
                    {
                        return new VindaxRBTDetailsDto();
                    }
                }
            }
            catch (Exception ex)
            {
                return new VindaxRBTDetailsDto();
            }
        }

        private async Task<LBANKRBTDetailsDto> GetRBTInfoLBank()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.lbkex.com/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync("/v2/ticker/24hr.do?symbol=rbt_usdt");
                    var dataString = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(dataString))
                    {
                        var dataObj = JsonConvert.DeserializeObject<LBANKRBTDetailsDto>(dataString);
                        return dataObj;
                    }
                    else
                    {
                        return new LBANKRBTDetailsDto();
                    }
                }
            }
            catch (Exception ex)
            {
                return new LBANKRBTDetailsDto();
            }
        }


        private async Task<WhiteBITRBTResponse> getWhiteBITRBTInfo()
        {
            WhiteBITRBTResponse dataObj = new WhiteBITRBTResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://whitebit.com/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync("api/v1/public/ticker?market=RBT_USDT");
                    var dataString = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(dataString))
                    {
                        dataObj = JsonConvert.DeserializeObject<WhiteBITRBTResponse>(dataString);
                        return dataObj;
                    }
                    else
                    {
                        return dataObj;
                    }
                }
            }
            catch (Exception ex)
            {
                return dataObj;
            }
        }


        [HttpGet]
        [Route("WhiteBit-GetRBTDetails")]
        public async Task<IActionResult> GetWhiteBITRBTDetails() 
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://whitebit.com/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync("api/v1/public/ticker?market=RBT_USDT");
                    var dataString = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(dataString))
                    {
                        var dataObj = JsonConvert.DeserializeObject<WhiteBITRBTResponse>(dataString);
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
        [HttpGet]
        [Route("getTopWallets")]
        public async Task<IActionResult> GetTopWallets()
        {
            try
            {
                
               var obj = await _repositoryRubixTransaction.GetTopWalletsAsync();
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("check-mined-status/{tokenhash}")]
        public async Task<IActionResult> IsTokenMined(string tokenhash)
        {
            try
            {
                if(tokenhash.Length == 67)
                {
                    var response = await _repositoryRubixToken.IsMinedToken(tokenhash);
                    if(response)
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
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,new
                {
                    status = false,
                    message =ex.Message
                });
            }
        }
    }
}
