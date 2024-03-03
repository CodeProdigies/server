using BCryptNet = BCrypt.Net.BCrypt;

namespace prod_server
{
    public class Utilities
    {

        public static string GetConnectionString()
        {
            return "Host=ruby.db.elephantsql.com;Database=gabgrurw;Username=gabgrur";
        }

        public static string HashPassword(string password)
        {
            return BCryptNet.HashPassword(password);
        }
    }
}
