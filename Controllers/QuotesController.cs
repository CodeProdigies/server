using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using prod_server.Classes;
using prod_server.Entities;
using prod_server.Services.DB;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using static prod_server.Classes.BaseController;
using static prod_server.Entities.Account;

namespace prod_server.Controllers
{
    [Authorize(AuthenticationSchemes = "Accounts")]
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IQuoteService _quoteService;

        public QuotesController(IAccountService accountService, IQuoteService quoteService)
        {
            _accountService = accountService;
            _quoteService = quoteService;
        }

        [EnableRateLimiting("quoteslimit")] // This will limit the amount of requests to 1 per 5 seconds.
        [HttpPost("/quote")]
        public async Task<IResponse<string>> Create([FromBody] Quote quote)
        {
            try
            {
                await _quoteService.Create(quote);
                return Ok<string>("quote_received");

            }
            catch (Exception e)
            {
                return new IResponse<string>();
            }

        }

        [HttpGet("/quote")]
        public async Task<IResponse<List<Quote>>> Get()
        {
            //get the user's permissions and check if they're admin. If admin, return all. If not, get the companyID filter with that.
            try
            {
                int? userId = null;
                var user = await _accountService.GetById();

                if (user == null) return new IResponse<List<Quote>>();
                if (user.Role != AccountRole.Admin) userId = user.Customer.Id;

                var quotes = await _quoteService.GetQuotes(userId);
                return Ok<List<Quote>>("quote_received", quotes);

            }
            catch (Exception e)
            {
                return new IResponse<List<Quote>>();
            }

        }

        [HttpGet("/quote/{id}")]
        public async Task<IResponse<Quote>> Get(Guid id)
        {
            try
            {
                var quote = await _quoteService.Get(id);
                return Ok<Quote>("quote_received", quote);

            }
            catch (Exception e)
            {
                return new IResponse<Quote>();
            }

        }

        [HttpDelete("/quote/{id}")]
        public async Task<IResponse<bool>> Delete(Guid id)
        {
            try
            {
                await _quoteService.Delete(id);
                return Ok<bool>("quote_deleted", true);

            }
            catch (Exception e)
            {
                return NotFound<bool>(e.Message, false);
            }

        }
    }
}
