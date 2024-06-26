using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EllipticCurve.Utils;
using Microsoft.Extensions.FileProviders;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace prod_server.Services
{
    public interface IUtilitiesService
    {
        public Task<SendGrid.Response> SendEmail(string to, string subject, string body);
        public Task<Azure.Response<BlobContentInfo>> UploadImage(byte[] Image, string imageName);
        public Task<Azure.Response<BlobContentInfo>> UploadImage(string Image, string imageName);
        public Task<Azure.Response<BlobContentInfo>> UploadFile(IFormFile file, string fileInfo);
        public Task<Azure.Response<BlobDownloadInfo>> GetFile(string filePath);

    }
    public class UtilitiesService : IUtilitiesService
    {
        private readonly string STORAGE_CONNECTION_LINK;
        private readonly string CONTAINER_NAME;
        private BlobContainerClient _BlobContainerClient { get; set; }
        public UtilitiesService()
        {
            STORAGE_CONNECTION_LINK = Environment.GetEnvironmentVariable("STORAGE_CONNECTION_LINK");
            CONTAINER_NAME = Environment.GetEnvironmentVariable("CONTAINER_NAME");
            _BlobContainerClient = new BlobContainerClient(STORAGE_CONNECTION_LINK, CONTAINER_NAME);

        }
        public Task<SendGrid.Response> SendEmail(string toEmail, string subject, string body)
        {

            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("noreply@valeteer.com", "Medsource");
            var to = new EmailAddress(toEmail, "aa");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            return client.SendEmailAsync(msg);
        }
        public async Task<Azure.Response<BlobContentInfo>> UploadImage(byte[] Image, string imageName)
        {
            // Save image into blob storage
            var containerClient = new BlobContainerClient(STORAGE_CONNECTION_LINK, CONTAINER_NAME);
            containerClient.CreateIfNotExists();
            var blobClient = containerClient.GetBlobClient(imageName);
            return await blobClient.UploadAsync(new MemoryStream(Image), true);
        }

        public async Task<Azure.Response<BlobContentInfo>> UploadImage(string Image, string imageName)
        {
            // Save image into blob storage
            var containerClient = new BlobContainerClient(STORAGE_CONNECTION_LINK, CONTAINER_NAME);
            containerClient.CreateIfNotExists();
            var blobClient = containerClient.GetBlobClient(imageName);
            return await blobClient.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(Image)), true);
        }

        public async Task<Azure.Response<BlobContentInfo>> UploadFile(IFormFile file, string fileInfo)
        {
            // Ensure the file is not null
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file), "The file to upload cannot be null.");
            }

            try
            {
                // Create the container if it doesn't exist
                await _BlobContainerClient.CreateIfNotExistsAsync();

                // Get a reference to the blob client using the fileInfo as the blob name
                var blobClient = _BlobContainerClient.GetBlobClient(fileInfo);

                // Open a seekable stream
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin); // Ensure stream position is at the beginning

                    // Upload the seekable stream to Azure Blob Storage
                    return await blobClient.UploadAsync(memoryStream, true);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new InvalidOperationException("Failed to upload the file.", ex);
            }

        }

        public async Task<Azure.Response<BlobDownloadInfo>> GetFile (string filePath)
        {
            var containerClient = new BlobContainerClient(STORAGE_CONNECTION_LINK, CONTAINER_NAME);
            var blobClient = containerClient.GetBlobClient(filePath);
            return await blobClient.DownloadAsync();
        }
    }

}
