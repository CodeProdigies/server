using System.Net.Mail;

namespace prod_server.Classes.Others
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public bool isPasswordValid()
        {
            return Password.Length > 8;
        }

        public bool IsValidEmail()
        {
            try
            {
                MailAddress mailAddress = new MailAddress(Email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
