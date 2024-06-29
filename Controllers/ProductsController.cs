using Azure;
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
            try
            {
                product.Files = new List<UploadedFile>();
                var newProduct = await _productService.Update(product);
                if (newProduct == 0) return UnexpectedError<Product>("failed_update_product");

                return Ok<Product>("product_update_successfully", product);

            } catch(Exception ex)
            {

               return UnexpectedError<Product>($"failed_update_product: {ex.Message}");
            }

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
            try { 
                var product = await _productService.GetById(productId);
                if (product == null) return null;

                var file = await _utilitiesService.GetFile($"/Products/{productId}/{image}");

                if (file == null) return null;

                return file.Value.Content;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost("{productId}/images")]
        [ProducesResponseType(typeof(IResponse<Product>), 400)]
        [ProducesResponseType(typeof(IResponse<Product>), 401)]
        [ProducesResponseType(typeof(IResponse<Product>), 200)]
        public async Task<IResponse<List<UploadedFile>>> UploadProductImages(UploadProductFiles request)
        {
            try
            {
                var product = await _productService.GetById(request.ProductId);
                if (product == null) return BadRequest<List<UploadedFile>>("failed_upload_product_notfound");

                var uploadedFiles = new List<UploadedFile>();

                foreach (var item in request.Files)
                {
                    if (item.Length == 0) return BadRequest<List<UploadedFile>>("failed_upload_product_image_empty");
                    if (item.Length > 5 * 1024 * 1024) return BadRequest<List<UploadedFile>>("failed_upload_product_image_too_large");
                    if (!item.ContentType.Contains("image")) return BadRequest<List<UploadedFile>>("failed_upload_product_image_invalid_format");

                    Guid FileGuid = Guid.NewGuid();
                    var fileName = $"{FileGuid} - {item.FileName}";

                    var uploadResult = await _utilitiesService.UploadFile(item, $"/Products/{request.ProductId}/{fileName}");
                    if (uploadResult.GetRawResponse().Status != 201) return BadRequest<List<UploadedFile>>("failed_upload_product_image");

                    uploadedFiles.Add(new UploadedFile(){
                        ContentType = item.ContentType,
                        Name = fileName,
                        Id= FileGuid,
                        FilePath = $"/Products/{request.ProductId}/{fileName}",
                        Size = item.Length  

                    });
                }


                if (uploadedFiles.Count == 0) return BadRequest<List<UploadedFile>>("failed_upload_product_images");
                await _productService.AddProductImages(product.Id!.Value, uploadedFiles);
                
                return Ok<List<UploadedFile>>("product_images_uploaded_successfully", uploadedFiles);

            }
            catch (Exception ex)
            {
                // Log the exception details to help with debugging
                // Consider using a logging framework or service
                return UnexpectedError<List<UploadedFile>>($"failed_upload_product_image: {ex.Message}");
            }
        }

        [HttpDelete("{productId}/image/{image}")]
        [ProducesResponseType(typeof(IResponse<Product>), 400)]
        [ProducesResponseType(typeof(IResponse<Product>), 401)]
        [ProducesResponseType(typeof(IResponse<Product>), 200)]
        public async Task<IResponse<bool>> DeleteProductImage(Guid productId, string? image)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(image)) return BadRequest<bool>("image_name_invalid");

                var product = await _productService.GetById(productId);
                if (product == null) return BadRequest<bool>("failed_delete_product_notfound");

                var file = product.Files.FirstOrDefault(x => x.Name == image);
                if(file == null) return BadRequest<bool>("failed_delete_product_image_notfound");
                 
                var deletionResult = await _utilitiesService.DeleteFile(file.FilePath!);
                if (!deletionResult) return BadRequest<bool>("failed_delete_file");

                product.Files.Remove(file);
                // Update the product if the file was successfully deleted
                await _productService.Update(product);

                return Ok<bool>("product_image_deleted_successfully", true);
            }
            catch (Exception ex)
            {
                // Log the exception details to help with debugging
                // Consider using a logging framework or service
                return UnexpectedError<bool>($"failed_delete_product_image: {ex.Message}");
            }
        }

        


        //[HttpGet("/products/images")]
        //[ProducesResponseType(typeof(IResponse<Product>), 500)]
        //[ProducesResponseType(typeof(IResponse<Product>), 401)]
        //[ProducesResponseType(typeof(IResponse<Product>), 200)]
        //public async Task<IResponse<List<Stream>>> GetProductImages(Guid productId)
        //{
        //    var product = await _productService.GetById(productId);
        //    if (product == null) return new IResponse<List<Stream>>() { Payload = new List<Stream>() };


        //    var fileStreams = await Task.WhenAll(product.Files.Select(async file =>
        //         await _utilitiesService.GetFile($"file.FilePath")
        //     ));

        //    var validStreams = fileStreams.Where(fs => fs != null).Select(fs => fs.Value.Content).ToList();

        //    return new IResponse<List<Stream>> { Payload = validStreams };
        //}
    }
}
