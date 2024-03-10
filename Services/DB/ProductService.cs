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
        public Task<Product?> GetBySKU(string sku);
        public Task<List<Product>> GetAll();
        public Task<int> Update(Product product);
        public Task<int> Delete(Guid id);
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
        public Task<Product?> GetBySKU(string sku)
        {
            return _database.Products.FirstOrDefaultAsync(x => x.SKU == sku);
        }
        public Task<List<Product>> GetAll()
        {
            return _database.Products.ToListAsync();
        }
        public Task<int> Update(Product product)
        {
            // Assuming product is tracked by the context (either attached or loaded)
            _database.Products.Update(product);

            return _database.SaveChangesAsync();
        }

        public async Task<int> Delete(Guid id)
        {
            var productToDelete = await _database.Products.FindAsync(id);

            if (productToDelete != null)
            {
                _database.Products.Remove(productToDelete);
                return await _database.SaveChangesAsync();
            }
            return 0; // Return 0 if the product with the specified id is not found
        }
    }
}
