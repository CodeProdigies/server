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

        public Task<Product> Create(CreateProductRequest request);
        public Task<Product?> GetById(Guid id);
        public Task<Product?> GetBySKU(string sku);
        public Task<List<Product>> GetAll();
        public Task<int> Update(Product product, List<UploadedFile>? files = null);
        public Task<int> AddProductImages(Guid ProductId, List<UploadedFile> newfiles);
        public Task<int> Delete(Guid id);
        public Task<int> Archive(Guid id, bool archive);
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
            return product;
        }

        public async Task<Product> Create(CreateProductRequest request)
        {
            var product = request.Product;
            if(request.Files != null && request.Files.Count > 0)
            {
                if(product.Files == null) product.Files = new List<UploadedFile>();
                foreach (var file in request.Files)
                {
                    // Save files
                    Guid FileGuid = Guid.NewGuid();
                    var fileName = $"{FileGuid} - {file.FileName}";
                    var fileData = new UploadedFile()
                    {
                        ProductId = product.Id,
                        Id= FileGuid,   
                        ContentType = file.ContentType,
                        Name = file.FileName,
                        FilePath = $"/Products/{product.Id}/{fileName}",
                        Size = file.Length

                    };

                    product.Files.Add(fileData);

                    var result = await _utilitiesService.UploadFile(file, fileData.FilePath);
                    
                }
   
            }

            await _database.Products.AddAsync(product);
            var response = await _database.SaveChangesAsync();
            return product;
        }

        public Task<Product?> GetById(Guid id)
        {
            _database.ChangeTracker.LazyLoadingEnabled = false;
            return _database.Products
                .Include(x => x.Provider)
                .Include(a => a.Files)
                .Where(b => b.isArchived == false)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public Task<Product?> GetBySKU(string sku)
        {
            return _database.Products.Where(b => b.isArchived == false).FirstOrDefaultAsync(x => x.SKU == sku);
        }
        public Task<List<Product>> GetAll()
        {
            return _database.Products.Include(a => a.Files).Where(b => b.isArchived == false).ToListAsync();
        }

        public Task<int> AddProductImages(Guid ProductId, List<UploadedFile> newfiles)
        {
            try
            {
                // add the non existant files to the database
                foreach (var file in newfiles)
                {
                    if (file.ProductId == null)
                    {
                        file.ProductId = ProductId;
                    }
                }
                _database.UploadedFiles.AddRange(newfiles);

                return _database.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public Task<int> Update(Product product, List<UploadedFile>? newfiles = null)
        {
            try
            {

                if(newfiles != null)
                {
                    // add the non existant files to the database
                    foreach (var file in newfiles)
                    {
                        if (file.ProductId == null) { 
                            file.ProductId = product.Id;
                        }
                    }
                   if (newfiles.Count > 0) product.Files.AddRange(newfiles);

                   _database.UploadedFiles.AddRange(newfiles);

                }

                // Assuming product is tracked by the context (either attached or loaded)
                _database.Products.Update(product);

                return _database.SaveChangesAsync();

            } 
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Delete(Guid id)
        {
            var productToDelete = await _database.Products.FindAsync(id);

            _database.UploadedFiles.RemoveRange(productToDelete.Files);
            await _database.SaveChangesAsync();

            if (productToDelete != null)
            {

                // Check if customer has any relationships with other entities
                var navigations = _database.Entry(productToDelete)
                    .Metadata
                    .GetNavigations()
                    .Where(n => !n.IsCollection);

                foreach (var navigation in navigations)
                {
                    var referenceEntry = _database.Entry(productToDelete).Reference(navigation.Name);
                    await referenceEntry.LoadAsync();
                    if (referenceEntry.CurrentValue != null)
                    {
                        // Product has related entities, handle accordingly (throw exception, log, etc.)
                        throw new InvalidOperationException("Product cannot be deleted because it has related entities.");
                    }
                }

                _database.UploadedFiles.RemoveRange(productToDelete.Files);
                _database.Products.Remove(productToDelete);

                return await _database.SaveChangesAsync();
            }
            return 0; // Return 0 if the product with the specified id is not found
        }

        public async Task<int> Archive(Guid id, bool archive)
        {
            var productToDelete = await _database.Products.FindAsync(id);
            if (productToDelete == null) return 0;

            productToDelete.isArchived = archive;
            _database.Products.Update(productToDelete);
            return await _database.SaveChangesAsync();

        }

        public Task<List<Product>> GetLastest(int quantity)
        {
            if (quantity > 25) quantity = 25;
            return _database.Products.OrderByDescending(x => x.CreatedAt).Where(b => b.isArchived == false).Take(quantity).ToListAsync();
        }
    }
}
