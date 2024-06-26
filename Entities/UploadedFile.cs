using Microsoft.Extensions.FileProviders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO; // For Path.GetExtension

namespace prod_server.Entities
{
    [Table("UploadedFiles")]
    public class UploadedFile
    {
        public UploadedFile() { }
        public UploadedFile(IFormFile file, string filePath = "/GenericUploads")
        {
            Name = file.FileName ?? throw new ArgumentNullException(nameof(file.FileName));
            ContentType = file.FileName.Split('.').Length > 1 ? file.FileName.Split('.')[1] : null;
            FilePath = filePath;
            Size = file.Length;
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        [Column("content_type")]
        public string? ContentType { get; set; }
        [Column("product_id")]
        public Guid? ProductId { get; set; }
        [Column("customer_id")]
        public int? CustomerId { get; set; }
        [Column("file_path")]
        public string? FilePath { get; set; }
        [Column("size")]
        public long Size { get; set; }


        public string GetFileType()
        {
            // More reliable way to get the file extension
            return Path.GetExtension(Name)?.TrimStart('.') ?? string.Empty;
        }
    }
}