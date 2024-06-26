using Microsoft.Extensions.FileProviders;
using prod_server.Entities;

namespace prod_server.Classes.Others
{
    public class CreateProductRequest
    {
        public Product Product { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
