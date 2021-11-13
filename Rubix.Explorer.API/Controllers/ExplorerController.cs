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

        private readonly IClientSessionHandle _clientSessionHandle;

        public ExplorerController(IRepositoryRubixUser repositoryUser, IRepositoryRubixToken repositoryRubixToken, IRepositoryRubixTokenTransaction repositoryRubixTokenTransaction, IRepositoryRubixTransaction repositoryRubixTransaction, IClientSessionHandle clientSessionHandle, IRepositoryDashboard repositoryDashboard) =>
            (_repositoryUser, _repositoryRubixToken, _repositoryRubixTokenTransaction, _repositoryRubixTransaction, _clientSessionHandle,_repositoryDashboard) = (repositoryUser, repositoryRubixToken, repositoryRubixTokenTransaction, repositoryRubixTransaction, clientSessionHandle, repositoryDashboard);

       

        [HttpGet]
        [Route("Cards")]
        public async Task<IActionResult> GetCardsData([FromQuery]ActivityFilter input)
        {
            try
            {
                var output = new RubixAnalyticsDto
                {
                    RubixPrice =0,
                    TransactionsCount =await _repositoryRubixTransaction.GetCountByFilterAsync(input),
                    TokensCount = await _repositoryRubixToken.GetCountByFilterAsync(input),
                    RubixUsersCount = await _repositoryUser.GetCountByFilterAsync(input)
                };
                return StatusCode(StatusCodes.Status200OK, output);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("LatestTokens")]
        public async Task<IActionResult> GetLatestTokens()
        {
            try
            {
                var latestTokens = await _repositoryRubixToken.GetPagerResultAsync(1,10);
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
                if(input.Page==0)
                {
                    input.Page = 1;
                }
                var latestTransactions = await _repositoryRubixTransaction.GetPagerResultAsync(input.Page,input.PageSize);
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
    }
}
