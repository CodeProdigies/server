using Microsoft.EntityFrameworkCore;
using prod_server.Classes.Others;
using prod_server.database;
using prod_server.Entities;
using System.Security.Claims;

namespace prod_server.Services.DB
{
    public interface IProductService : IService<Order>
    {
        public Task<Product> Create(Product product);
        public Task<Product?> GetById(Guid id);
        public Task<Product?> GetBySKU(string sku);
        public Task<List<Product>> GetAll();
        public Task<int> Update(Product product);
        public Task<int> Delete(Guid id);
        public Task<List<Product>> GetLastest(int quantity);
    }

    public class ProductService : Service<Order>, IProductService
    {
        private readonly IUtilitiesService _utilitiesService;
        public ProductService(Context database, IHttpContextAccessor contextAccessor, IUtilitiesService utilitiesService)
            : base(database, contextAccessor)
        {
            _utilitiesService = utilitiesService;
        }
        public async Task<Product> Create(Product product)
        {
            await _database.Products.AddAsync(product);
            var response = await _database.SaveChangesAsync();

            if (response == 1 && product.Image != null)
            {
                await _utilitiesService.uploadImage(product.Image, $"{product.SKU}.png");
            }
            return product;
        }

        public Task<Product?> GetById(Guid id)
        {
            _database.ChangeTracker.LazyLoadingEnabled = false;
            return _database.Products.Include(x => x.Provider).FirstOrDefaultAsync(x => x.Id == id);
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

        public Task<List<Product>> GetLastest(int quantity)
        {
            if (quantity > 25) quantity = 25;
            return _database.Products.OrderByDescending(x => x.CreatedAt).Take(quantity).ToListAsync();
        }
    }
}
