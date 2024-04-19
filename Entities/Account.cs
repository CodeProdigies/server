using Microsoft.IdentityModel.Tokens;
using prod_server.Classes.Others;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using BCryptNet = BCrypt.Net.BCrypt;

namespace prod_server.Entities
{
    [Table("Accounts")]
    public class Account
    {
        public Account() { }
        public Account (RegisterModel registerModel)
        {
            Username = registerModel.Username;
            Password = registerModel.Password;
            Email = registerModel.Email;
            FirstName = registerModel.FirstName;
            LastName = registerModel.LastName;
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        [MaxLength(50)]
        public string Username { get; set; }

        [JsonIgnore]
        [Column("password")]
        /// <summary> Password is hashed and salted. </summary>
        public string? Password { get; set; }

        [Required]
        [Column("email_address")]
        [MaxLength(50)]
        public string Email { get; set; }

        [AllowNull]
        [Column("first_name")]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [AllowNull]
        [Column("last_name")]
        [MaxLength(50)]
        public string? LastName { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updatedAt")]
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
        [AllowNull]
        [Column("address")]
        public string? Address { get; set; }
        [AllowNull]
        [Column("city")]
        public string? City { get; set; }
        [AllowNull]
        [Column("state")]
        public string? State { get; set; }
        [AllowNull]
        [Column("zipCode")]
        public string? ZipCode { get; set; }
        [AllowNull]
        [Column("country")]
        public string? Country { get; set; }
        [AllowNull]
        [Column("phone")]
        public string? Phone { get; set; }
        [AllowNull]
        [Column("mobile")]
        public string? Mobile{ get; set; }

        public virtual List<Notification> Notifications { get; set; } = new List<Notification>();


        public string CreateJwtToken()
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("33d7b68dc5eab0934f001fc5801bd234a255aa0e7314cb43619e5604001132ece77b4a73f0fd8c67d89e43662d2d968d2b18bd20c8dbc78ac5512ccf41f381cb"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var jwtToken = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: credentials,
                    issuer: "localhost",
                    audience: "localhost"
                );
            var jetTokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return jetTokenString;
        }

        public bool ValidatePassword (string password)
        {
            return BCryptNet.Verify(password, Password);
        }

        public void UpdateFromAnotherAccount(Account acc)
        {
            this.Username = acc.Username;
            this.Email = acc.Email;
            this.FirstName = acc.FirstName;
            this.LastName = acc.LastName;
            this.Phone = acc.Phone;
            this.Mobile = acc.Mobile;
            this.Address = acc.Address;
            this.City = acc.City;
            this.State = acc.State;
            this.ZipCode = acc.ZipCode;
            this.Country = acc.Country;
                
            this.UpdatedAt = DateTime.UtcNow;
                
        }
    }
}
