using Microsoft.EntityFrameworkCore;
using prod_server.database;
using prod_server.Entities;

namespace prod_server.Services.DB
{
    public interface ICustomerService
    {
        public Task<Customer> Create(Customer customer);
        public Task<Customer?> GetCustomer(int id);
        public Task<List<Customer>> GetAllCustomers();
        public Task<int> UpdateCustomer(Customer customer);
        public Task<int> DeleteCustomer(int id);

    }
    public class CustomerService : ICustomerService
    {
        private readonly Context _database;

        public CustomerService(Context database)
        {
            _database = database;
        }

        public async Task<Customer> Create(Customer customer)
        {
            await _database.Customers.AddAsync(customer);
            await _database.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> GetCustomer(int id)
        {
            return await _database.Customers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _database.Customers.ToListAsync();
        }

        public async Task<int> UpdateCustomer(Customer customer)
        {
            _database.Customers.Update(customer);
            return await _database.SaveChangesAsync();
        }

        public async Task<int> DeleteCustomer(int id)
        {
            var customerToDelete = await _database.Customers.FindAsync(id);

            if (customerToDelete != null)
            {
                _database.Customers.Remove(customerToDelete);
                return await _database.SaveChangesAsync();
            }
            return 0; // Return 0 if the product with the specified id is not found
        }
    }
}
