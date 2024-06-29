namespace prod_server.Classes.Others
{
    public class UploadProductFiles
    {
        public UploadProductFiles() {}
        public Guid ProductId { get; set; }
        public List<IFormFile> Files { get; set; } = [];
    }
}
