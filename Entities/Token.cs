using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod_server.Entities
{
    public enum OTPType
    {
        ForgotPassword,
        Login
    }
    [Table("Tokens")]
    public class Token
    {
        public Token() { }
        public Token(Account account, OTPType? type = null)
        {
            int minutesValid = 5;

            switch(type)
            {
                case OTPType.ForgotPassword:
                    minutesValid = 30;
                    break;
                case OTPType.Login:
                    minutesValid = 5;
                    break;
            }

            Code = Utilities.GenerateRandomNumber(6);
            UserId = account.Id.ToString();
            CreatedAt = DateTime.UtcNow;
            ExpiresAt = DateTime.UtcNow.AddMinutes(minutesValid);
            Type = type ?? OTPType.Login;
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Column("code")]
        public string? Code { get; set; }
        [Column("email")]
        public string? Email { get; set; }
        [Column("userId")]
        public string? UserId { get; set; }
        [Column("type")]
        public OTPType Type { get; set; }
        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }
        [Column("expiresAt")]
        public DateTime ExpiresAt { get; set; }
    }
}
