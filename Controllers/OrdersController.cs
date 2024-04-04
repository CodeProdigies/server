using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prod_server.Classes;
using prod_server.Entities;
using prod_server.Services.DB;
using static prod_server.Classes.BaseController;

namespace prod_server.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;

        public OrdersController(IAccountService accountService, IOrderService quoteService)
        {
            _accountService = accountService;
            _orderService = quoteService;
        }

        [HttpPost("/orders")]
        public async Task<IResponse<string>> Create([FromBody] Order quote)
        {
            try
            {
                await _orderService.Create(quote);
                return Ok<string>("quote_received");

            }
            catch (Exception e)
            {
                return new IResponse<string>();
            }

        }

        [HttpGet("/orders")]
        public async Task<IResponse<List<Order>>> Get()
        {
            try
            {
                var Orders = await _orderService.GetAll();
                return Ok<List<Order>>("quote_received", Orders);

            }
            catch (Exception e)
            {
                return new IResponse<List<Order>>();
            }

        }

        [HttpGet("/orders/{id}")]
        public async Task<IResponse<Order>> Get(int id)
        {
            try
            {
                var Orders = await _orderService.Get(id);
                return Ok<Order>("quote_received", Orders);

            }
            catch (Exception e)
            {
                return new IResponse<Order>();
            }

        }

        [HttpDelete("/orders/{id}")]
        public async Task<IResponse<bool>> Delete(int id)
        {
            try
            {
                await _orderService.Delete(id);
                return Ok<bool>("quote_deleted", true);

            }
            catch (Exception e)
            {
                return NotFound<bool>(e.Message, false);
            }

        }
    }
}
