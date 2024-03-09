using Microsoft.EntityFrameworkCore;
using prod_server.Classes.Others;
using prod_server.database;
using prod_server.Entities;
using System.Security.Claims;

namespace prod_server.Services.DB
{
    public interface IProductService
    {
        public Task<Product> Create(Product product);
        public Task<Product?> GetById(Guid id);
    }
  
    public class ProductService : IProductService
    {
        private readonly Context _database;

        public ProductService(Context database)
        {
            _database = database;
        }

        public async Task<Product> Create(Product product)
        {
            await _database.Products.AddAsync(product);
            await _database.SaveChangesAsync();
            return product;
        }
        
        public Task<Product?> GetById(Guid id)
        {
            return _database.Products.FirstOrDefaultAsync(x => x.Id == id);
        }
        
    }
}
