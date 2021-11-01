using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rubix.API.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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


        private readonly IClientSessionHandle _clientSessionHandle;

        public ExplorerController(IRepositoryRubixUser repositoryUser, IRepositoryRubixToken repositoryRubixToken, IRepositoryRubixTokenTransaction repositoryRubixTokenTransaction, IRepositoryRubixTransaction repositoryRubixTransaction, IClientSessionHandle clientSessionHandle) =>
            (_repositoryUser, _repositoryRubixToken, _repositoryRubixTokenTransaction, _repositoryRubixTransaction, _clientSessionHandle) = (repositoryUser, repositoryRubixToken, repositoryRubixTokenTransaction, repositoryRubixTransaction, clientSessionHandle);

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _repositoryUser.GetPagerResultAsync(2,10);

            return Ok();
        }
    }
}
