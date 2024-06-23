using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prod_server.Classes;
using prod_server.Classes.Others;
using prod_server.Entities;
using prod_server.Services.DB;
using System.Text;
using static prod_server.Classes.BaseController;

namespace prod_server.Controllers
{
    [Authorize(AuthenticationSchemes = "Accounts")]
    [ApiController]

    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IQuoteService _quoteService;
        private readonly IAccountService _accountService;

        public OrdersController(IOrderService orderService, IQuoteService quoteService, IAccountService accountService)
        {
            _orderService = orderService;
            _quoteService = quoteService;
            _accountService = accountService;
        }

        [HttpPost("/orders")]
        public async Task<IResponse<string>> Create([FromBody] CreateQuoteRequest order)
        {
            {
                await _orderService.Create(new Order(order));
                return Ok<string>("quote_received");

            }
            catch (Exception e)
            {
                return new IResponse<string>();
            }

        }

        [HttpPut("/orders")]
        public async Task<IResponse<string>> Update([FromBody] Order order)
        {
            try
            {
                await _orderService.Update(order);
                return Ok<string>("quote_updated");

            }
            catch (Exception e)
            {
                return new IResponse<string>();
            }

        }

        [HttpPost("/orders/fromquote/{quoteId}")]
        public async Task<IResponse<Order>> Create(string quoteId)
        {
            try
            {

                var quote = await _quoteService.Get(Guid.Parse(quoteId));

                if (quote == null) return NotFound<Order>("quote_not_found");

                var order = new Order(quote);
                await _orderService.Create(order);
                return Ok<Order>("order_created", order);

            }
            catch (Exception e)
            {
                return new IResponse<Order>();
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

        [HttpGet("/orders/fromcustomer/{customerId}")]
        public async Task<IResponse<List<Order>>> GetFromCustomer(int customerId)
        {
            try
            {
                var Orders = await _orderService.GetFromCustomerId(customerId);
                return Ok<List<Order>>("quote_received", Orders);

            }
            catch (Exception e)
            {
                return new IResponse<List<Order>>();
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

        [HttpPost("/orders/search")]
        public async Task<IResponse<PagedResult<Order>>> Get(GenericSearchFilter request)
        {
            var user = await _accountService.GetById();
            if (user == null)
                return Unauthorized<PagedResult<Order>>("unauthorized_access");

            if (user.Role < Account.AccountRole.Admin && user.CustomerId.HasValue)
            {
                request.Filters.Add("CustomerId", user.CustomerId.Value.ToString());
            }
            else if (user.Role < Account.AccountRole.Admin)
            {
                return Unauthorized<PagedResult<Order>>("Unauthorized");
            }

            var result = await _orderService.Search(request);
            if (result == null) return NotFound<PagedResult<Order>>("Unexpected Server Error");

            return Ok<PagedResult<Order>>("quote_retreived_successfully", result);

        }
    }
}
