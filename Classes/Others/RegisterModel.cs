using prod_server.Classes.Common;
using System.Net.Mail;
using static prod_server.Entities.Account;

namespace prod_server.Classes.Others
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Name Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public AccountRole Role { get; set; } = AccountRole.Customer;

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
