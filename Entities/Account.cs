using prod_server.Classes.Others;

namespace prod_server.Entities
{
    public class Account
    {
        public Account (RegisterModel registerModel)
        {
            Username = registerModel.Username;
            Password = registerModel.Password;
            Email = registerModel.Email;
            FirstName = registerModel.FirstName;
            LastName = registerModel.LastName;
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
