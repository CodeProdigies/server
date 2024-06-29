using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using prod_server.Classes;
using prod_server.Entities;
using prod_server.Services;
using prod_server.Services.DB;

namespace prod_server.Controllers
{
    [ApiController]
    public class GeneralController : BaseController
    {
        private readonly IDocumentsService _documentsService;
        private readonly IQuoteService _quoteService;
        private readonly IAccountService _accountService;
        private readonly IUtilitiesService _utilitiesService;

        public GeneralController(IDocumentsService documentsService, IQuoteService quoteService, IAccountService accountService, IUtilitiesService utilitiesService)
        {
            _documentsService = documentsService;
            _quoteService = quoteService;
            _accountService = accountService;
            _utilitiesService = utilitiesService;
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

        // This endpoint must be secured at all times. Only authozied users can access it.
        [HttpGet("file/getfilebypath")]
        [ProducesResponseType(typeof(IResponse<Product>), 400)]
        [ProducesResponseType(typeof(IResponse<Product>), 401)]
        [ProducesResponseType(typeof(IResponse<Product>), 200)]
        public async Task<Stream?> Filebypath(string path)
        {
            try
            {
                var user = await _accountService.GetById();

                if (string.IsNullOrWhiteSpace(path)) return null;

                var file = await _utilitiesService.GetFile(path);
                if (file == null) return null;

                return file.Value.Content;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
