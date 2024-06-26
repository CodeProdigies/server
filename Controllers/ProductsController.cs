using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prod_server.Classes;
using prod_server.Classes.Others;
using prod_server.Entities;
using prod_server.Services;
using prod_server.Services.DB;
using System.Security.Claims;

namespace prod_server.Controllers
{
    [Authorize(AuthenticationSchemes = "Accounts")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IProductService _productService;
        private readonly IUtilitiesService _utilitiesService;

        public ProductsController(IAccountService accountService, IProductService productService, IUtilitiesService utilitiesService)
        {
            _accountService = accountService;
            _productService = productService;
            _utilitiesService = utilitiesService;
        }

        [AllowAnonymous]
        [HttpGet("/products/{id}")]
        [ProducesResponseType(typeof(IResponse<>), 404)]
        [ProducesResponseType(typeof(IResponse<Product>), 200)]
        public async Task<IResponse<Product?>> GetProduct(Guid id)
        {

            string? userId = this.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var account = await _accountService.GetByUsername(userId);

            var product = await _productService.GetById(id);

            if (product == null) return NotFound<Product?>("failed_retrieve_product");

            return Ok<Product?>("product_retrieved_successfully", product);
        }

        [HttpPost("/products")]
        [ProducesResponseType(typeof(IResponse<Product>), 400)]
        [ProducesResponseType(typeof(IResponse<Product>), 401)]
        [ProducesResponseType(typeof(IResponse<Product>), 200)]
        public async Task<IResponse<Product>> Create([FromForm] CreateProductRequest request)
        {
            var product = request.Product;
            if (product == null) return BadRequest<Product>("failed_create_product");

            if(request.Files != null)
            {
                var hasDuplicateFileName = request.Files.GroupBy(x => x.FileName).Any(g => g.Count() > 1);
                if (hasDuplicateFileName == true) return BadRequest<Product>("failed_create_product_duplicate_file_name");
            } 

            string? userId = this.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var account = await _accountService.GetById(userId!);
            if (account == null) return Unauthorized<Product>("failed_create_account");

            // Check if user is admin. When we do the roles.

            var newProduct = await _productService.Create(request);
            if (newProduct == null) return UnexpectedError<Product>("failed_create_product");

            return Ok<Product>("product_created_successfully", newProduct);
        }

        [HttpPut("/products")]
        [ProducesResponseType(typeof(IResponse<Product>), 400)]
        [ProducesResponseType(typeof(IResponse<Product>), 401)]
        [ProducesResponseType(typeof(IResponse<Product>), 200)]
        public async Task<IResponse<Product>> Update(Product? product)
        {
            if (product == null) return BadRequest<Product>("failed_update_product");
            if (product.Id == null) return BadRequest<Product>("failed_update_product");


            string? userId = this.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var account = await _accountService.GetById(userId!);
            if (account == null) return Unauthorized<Product>("failed_updat3e_account");

            // Check if user is admin. When we do the roles.

            var newProduct = await _productService.Update(product);
            if (newProduct == null) return UnexpectedError<Product>("failed_update_product");

            return Ok<Product>("product_update_successfully", product);
        }

        [HttpDelete("/products/{id}")]
        [ProducesResponseType(typeof(IResponse<Product>), 400)]
        [ProducesResponseType(typeof(IResponse<Product>), 401)]
        [ProducesResponseType(typeof(IResponse<Product>), 200)]
        public async Task<IResponse<string>> Delete(Guid id)
        {

            string? userId = this.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var account = await _accountService.GetById(userId!);
            if (account == null) return Unauthorized<string>("failed_delete_account");

            // Check if user is admin. When we do the roles.

            var newProduct = await _productService.Delete(id);

            if (newProduct == 0) return BadRequest<string>("failed_delete_product_notfound");

            return Ok<string>("product_deleted_successfully");
        }

        [AllowAnonymous]
        [HttpGet("/products")]
        [ProducesResponseType(typeof(IResponse<Product>), 500)]
        [ProducesResponseType(typeof(IResponse<Product>), 401)]
        [ProducesResponseType(typeof(IResponse<Product>), 200)]
        public async Task<IResponse<List<Product>>> GetAllProducts(Product? product)
        {

            // Check if user is admin. When we do the roles.

            var products = await _productService.GetAll();
            if (products == null) return UnexpectedError<List<Product>>("failed_retrieve_product");

            return Ok<List<Product>>("products_retrieved_successfully", products);
        }

        [AllowAnonymous]
        [HttpGet("/products/lastest")]
        [ProducesResponseType(typeof(IResponse<Product>), 500)]
        [ProducesResponseType(typeof(IResponse<Product>), 401)]
        [ProducesResponseType(typeof(IResponse<Product>), 200)]
        public async Task<IResponse<List<Product>>> GetLastest(int quantity = 3)
        {

            // Check if user is admin. When we do the roles.

            var products = await _productService.GetLastest(quantity);
            if (products == null) return UnexpectedError<List<Product>>("failed_retrieve_product");

            return Ok<List<Product>>("products_retrieved_successfully", products);
        }

        [AllowAnonymous]
        [HttpGet("/products/image")]
        [ProducesResponseType(typeof(IResponse<Product>), 500)]
        [ProducesResponseType(typeof(IResponse<Product>), 401)]
        [ProducesResponseType(typeof(IResponse<Product>), 200)]
        public async Task<Stream> GetProductImage(Guid productId, string image)
        {

            var file = await _utilitiesService.GetFile($"/Products/{productId}/{image}");

            if (file == null) return null;

            return file.Value.Content;
        }
    }
}
