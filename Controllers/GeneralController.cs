using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using prod_server.Classes;
using prod_server.Entities;
using prod_server.Services.DB;
using System.Collections.Generic;
using static prod_server.Classes.BaseController;

namespace prod_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : BaseController
    {
        private readonly IQuoteService _quoteService;

        public GeneralController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        [EnableRateLimiting("quoteslimit")] // This will limit the amount of requests to 1 per 5 seconds.
        [HttpPost("/quote")]
        public async Task<IResponse<string>> CreateQuote([FromBody] Quote quote)
        {
            try
            {
                await _quoteService.Create(quote);
                return Ok<string>("quote_received");

            }catch (Exception e)
            {
                return new IResponse<string>();
            }

        }

        [HttpGet("/quote")]
        public async Task<IResponse<List<Quote>>> GetQuote()
        {
            try
            {
                var quotes = await _quoteService.GetQuotes();
                return Ok<List<Quote>> ("quote_received", quotes);

            }
            catch (Exception e)
            {
                return new IResponse<List<Quote>>();
            }

        }

        [HttpGet("/quote/{id}")]
        public async Task<IResponse<Quote>> GetQuote(Guid id)
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
    }
}
