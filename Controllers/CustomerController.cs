using Microsoft.AspNetCore.Mvc;
using prod_server.Classes;
using prod_server.Classes.Others;
using prod_server.Entities;
using prod_server.Services.DB;

namespace prod_server.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
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
    }
}
