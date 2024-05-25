using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prod_server.Classes;
using prod_server.Classes.Others;
using prod_server.Entities;
using prod_server.Services.DB;

namespace prod_server.Controllers
{
    [Authorize(AuthenticationSchemes = "Accounts")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _contextAccessor;

        public CustomerController(ICustomerService customerService, IOrderService orderService, IAccountService accountService, IHttpContextAccessor contextAccessor)
        {
            _customerService = customerService;
            _orderService = orderService;
            _accountService = accountService;
            _contextAccessor = contextAccessor;
        }

        [HttpPost("customer")]
        public async Task<IResponse<Customer>> CreateCustomer([FromBody]CustomerModel customerModel)
        {
            var customer = new Customer(customerModel);
            var result = await _customerService.Create(customer);
            return Ok<Customer>("customer_created_successfully", result);
        }

        [HttpGet("customer/{id}")]
        public async Task<IResponse<Customer>> GetCustomer(int id)
        {
            var result = await _customerService.GetCustomer(id);
            if (result == null) return NotFound<Customer>("customer_not_found");

            return Ok<Customer>("customer_retreived_successfully", result);
        }

        [HttpGet("customers")]
        public async Task<IResponse<List<Customer>>> GetAllCustomers()
        {
            var result = await _customerService.GetAllCustomers();
            return Ok<List<Customer>>("customers_retreived_successfully", result);
        }

        [HttpPut("customer")]
        public async Task<IResponse<bool>> UpdateCustomer([FromBody] Customer customer)
        {
            var result = await _customerService.UpdateCustomer(customer);
            var updated = result > 0;
            var message = updated ? "customer_updated_successfully" : "customer_not_found";
            return Ok<bool>(message, updated);
        }

        [HttpDelete("customer/{id}")]
        public async Task<IResponse<bool>> DeleteCustomer(int id)
        {
            var result = await _customerService.DeleteCustomer(id);
            var deleted = result > 0;
            var message = deleted ? "customer_deleted_successfully" : "customer_not_found";
            return Ok<bool>(message, deleted);
        }

        [HttpGet("customer/{id}/orders")]
        public async Task<IResponse<List<Order>>> GetCustomerOrders(int id)
        {
            var result = await _orderService.GetByCustomer(id);
            return Ok<List<Order>>("customer_orders_retreived_successfully", result);
        }

        [HttpPost("customer/search")]
        public async Task<IResponse<PagedResult<Customer>>> GetCustomerByUser([FromBody]GenericSearchFilter request)
        {
            var user = await _accountService.GetById();
            if(user == null || user.Role < Account.AccountRole.Admin) return Unauthorized<PagedResult<Customer>>("unauthorized_access", null);

            var result = await _customerService.Search(request);
            if (result == null) return NotFound<PagedResult<Customer>>("Unexpected Server Error");

            return Ok<PagedResult<Customer>>("customer_retreived_successfully", result);
        }
    }
}
