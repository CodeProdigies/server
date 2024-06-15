using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;

namespace prod_server.Services
{
    public interface IUtilitiesService
    {
        public Task<Response> SendEmail(string to, string subject, string body);

    }
    public class UtilitiesService : IUtilitiesService
    {
        public Task<Response> SendEmail(string toEmail, string subject, string body)
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
    }
}
