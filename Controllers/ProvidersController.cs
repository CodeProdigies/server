using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prod_server.Classes;
using prod_server.database;
using prod_server.Entities;
using prod_server.Services.DB;

namespace prod_server.Controllers
{
    [Authorize(AuthenticationSchemes = "Accounts")]
    [ApiController]
    public class ProvidersController : BaseController
    {
        private readonly IProviderService _providerService;
        private readonly IHttpContextAccessor _contextAccessor;

        public ProvidersController(IHttpContextAccessor contextAccessor, IProviderService providerService)
        {
            _providerService = providerService;
            _contextAccessor = contextAccessor;
        }
        // POST: ProviderControllercs/Create
        [HttpPost("provider")]
        public async Task<IResponse<Provider?>> Create([FromBody] Provider provider)
        {
            var newProvider = await _providerService.Create(provider);
            return Ok<Provider>("Created Successfully", newProvider);
            
        }

        [HttpPut("provider")]
        public async Task<IResponse<bool>> Edit([FromBody] Provider provider)
        {
            bool edited = await _providerService.Update(provider) > 0;
            return Ok<bool>("Updated Successfully", edited);
        }

        [HttpDelete("provider/{id}")]
        public async Task<IResponse<bool>> Delete(int id)
        {
            bool deleted = await _providerService.Delete(id) > 0;
            return Ok<bool>("Deleted Successfully", deleted);
        }

        [HttpGet("provider/{id}")]
        public async Task<IResponse<Provider?>> Get(int id)
        {
            var provider = await _providerService.Get(id);
            return Ok<Provider?>("Found", provider);
        }

        [HttpGet("providers")]
        public async Task<IResponse<List<Provider>>> GetAll()
        {
            var providers = await _providerService.GetAll();
            return Ok<List<Provider>>("Found", providers);
        }


    }
}
