using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using prod_server.Classes;
using prod_server.Services;
using prod_server.Services.DB;

namespace prod_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : BaseController
    {
        private readonly IDocumentsService _documentsService;
        private readonly IQuoteService _quoteService;

        public GeneralController(IDocumentsService documentsService, IQuoteService quoteService)
        {
            _documentsService = documentsService;
            _quoteService = quoteService;
        }

        [HttpGet("/p/ping")]
        public async Task<IActionResult> Ping()
        {
            var quotes = await _quoteService.GetQuotes();
            var quote = quotes.FirstOrDefault();

            var document = await _documentsService.CreateQuote(quote);

            // Set the content disposition header so the browser knows it's a file
            var contentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Quote.pdf"
            };
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

            return File(document, "application/pdf");
        }
    }
}
