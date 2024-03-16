using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using prod_server.Entities;
using prod_server.Services.DB;
using static prod_server.Classes.BaseController;

namespace prod_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : Controller
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
                return new IResponse<string>();

            }catch (Exception e)
            {
                return new IResponse<string>();
            }

        }
    }
}
