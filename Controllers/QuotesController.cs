using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using prod_server.Classes;
using prod_server.Classes.Others;
using prod_server.Entities;
using prod_server.Services.DB;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using static prod_server.Classes.BaseController;

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
                var user = await _accountService.GetById();

                if(user != null && user.Customer != null)
                {
                    quote.Name = user.Customer?.Name ?? "";
                    quote.EmailAddress = user.Email;
                    quote.ContactName = user.FirstName + " " + user.LastName;
                    quote.PhoneNumber = user.Phone;
                    quote.Customer = user.Customer;
                }


                await _quoteService.Create(quote);
                return Ok<string>("quote_received");

            }
            catch (Exception e)
            {
                return new IResponse<string>();
            }

        }

        //[HttpGet("/quote")]
        //public async Task<IResponse<List<Quote>>> Get()
        //{
        //    try
        //    {
        //        var quotes = await _quoteService.GetQuotes();
        //        return Ok<List<Quote>>("quote_received", quotes);

        //    }
        //    catch (Exception e)
        //    {
        //        return new IResponse<List<Quote>>();
        //    }

        //}

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

        [HttpPost("/quote/search")]
        public async Task<IResponse<PagedResult<Quote>>> Get(GenericSearchFilter request)
        {
            var user = await _accountService.GetById();
            if (user == null || user.Role < Account.AccountRole.Admin) return Unauthorized<PagedResult<Quote>>("unauthorized_access", null);

            var result = await _quoteService.Search(request);
            if (result == null) return NotFound<PagedResult<Quote>>("Unexpected Server Error");

            return Ok<PagedResult<Quote>>("quote_retreived_successfully", result);    

        }
    }
}
