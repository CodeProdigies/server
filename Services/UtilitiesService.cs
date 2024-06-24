using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
        public Task<Azure.Response<BlobContentInfo>> uploadImage(byte[] Image, string imageName);
        public Task<Azure.Response<BlobContentInfo>> uploadImage(string Image, string imageName);
        

    }
    public class UtilitiesService : IUtilitiesService
    {
        private readonly string STORAGE_CONNECTION_LINK;
        private readonly string CONTAINER_NAME;
        public UtilitiesService()
        {
            STORAGE_CONNECTION_LINK = Environment.GetEnvironmentVariable("STORAGE_CONNECTION_LINK");
            CONTAINER_NAME = Environment.GetEnvironmentVariable("CONTAINER_NAME");

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
        public async Task<Azure.Response<BlobContentInfo>> uploadImage(byte[] Image, string imageName)
        {
            // Save image into blob storage
            var containerClient = new BlobContainerClient(STORAGE_CONNECTION_LINK, CONTAINER_NAME);
            containerClient.CreateIfNotExists();
            var blobClient = containerClient.GetBlobClient(imageName);
            var response = await blobClient.UploadAsync(new MemoryStream(Image), true);

            var successfullyUploaded = response.GetRawResponse().Status == 201;
            var AzureResponseToHttpResponse = successfullyUploaded ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;

            return response;

        }

        public async Task<Azure.Response<BlobContentInfo>> uploadImage(string Image, string imageName)
        {
            // Save image into blob storage
            var containerClient = new BlobContainerClient(STORAGE_CONNECTION_LINK, CONTAINER_NAME);
            containerClient.CreateIfNotExists();
            var blobClient = containerClient.GetBlobClient(imageName);
            var response = await blobClient.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(Image)), true);

            var successfullyUploaded = response.GetRawResponse().Status == 201;
            var AzureResponseToHttpResponse = successfullyUploaded ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;

            return response;

        }
    }

}
