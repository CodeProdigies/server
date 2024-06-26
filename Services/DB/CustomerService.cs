﻿using Microsoft.EntityFrameworkCore;
using prod_server.Classes.Others;
using prod_server.database;
using prod_server.Entities;
using System.Reflection;

namespace prod_server.Services.DB
{
    public interface ICustomerService : IService<Customer>
    {
        public Task<Customer> Create(Customer customer);
        public Task<Customer?> GetCustomer(int id);
        public Task<List<Customer>> GetAllCustomers();
        public Task<int> UpdateCustomer(Customer customer);
        public Task<int> DeleteCustomer(int id);
    }
    public class CustomerService : Service<Customer>, ICustomerService
    {
        public CustomerService(Context database, IHttpContextAccessor contextAccessor) : base(database, contextAccessor) {}

        public async Task<Customer> Create(Customer customer)
        {
            await base._database.Customers.AddAsync(customer);
            await base._database.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> GetCustomer(int id)
        {
            return await base._database.Customers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return await base._database.Customers.ToListAsync();
        }

        public async Task<int> UpdateCustomer(Customer customer)
        {
            base._database.Customers.Update(customer);
            return await base._database.SaveChangesAsync();
        }

        public async Task<int> DeleteCustomer(int id)
        {
            var customerToDelete = await _database.Customers.FindAsync(id);

            if (customerToDelete != null)
            {
                // Check if customer has any relationships with other entities
                var relatedEntities = _database.Entry(customerToDelete)
                    .Metadata
                    .GetNavigations()
                    .Where(n => !n.IsCollection)
                    .Select(n => _database.Entry(customerToDelete)?.Reference(n.Name)?.TargetEntry?.Entity)
                    .ToList();

                if (relatedEntities.Any())
                {
                    // Customer has related entities, handle accordingly (throw exception, log, etc.)
                    throw new InvalidOperationException("Customer cannot be deleted because it has related entities.");
                }

                // No related entities found, proceed with deletion
                _database.Customers.Remove(customerToDelete);
                return await _database.SaveChangesAsync();
            }

            return 0; // Return 0 if the customer with the specified id is not found
        }
    }
}
