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

        public static string GenerateRandomNumber(int length)
        {
            Random random = new Random();
            string number = "";
            for (int i = 0; i < length; i++)
            {
                number += random.Next(0, 9).ToString();
            }
            return number;
        }
    }
}
