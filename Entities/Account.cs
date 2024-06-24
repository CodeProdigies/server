using Microsoft.IdentityModel.Tokens;
using prod_server.Classes.Common;
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
        public Account(RegisterModel registerModel)
        {
            Username = registerModel.Username;
            Password = registerModel.Password;
            Email = registerModel.Email;
            Name = registerModel.Name;
            DateOfBirth = registerModel.DateOfBirth;
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


        [Required]
        [Column("name")]
        public Name Name { get; set; } = new Name();

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updatedAt")]
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        [AllowNull]
        [Column("dob")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [Column("shipping_details")]
        public Address ShippingDetails { get; set; } = new Address();

        [AllowNull]
        [Column("phone")]
        public string? Phone { get; set; }

        [AllowNull]
        [Column("mobile")]
        public string? Mobile { get; set; }

        [Column("role")]
        public AccountRole Role { get; set; } = AccountRole.Customer;

        [AllowNull]
        [Column("customer_id")]
        public int? CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }

        public virtual List<Notification> Notifications { get; set; } = new List<Notification>();

        public enum AccountRole
        {
            Customer,
            Employee = 10,
            Admin = 9999999
        }

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

        public bool ValidatePassword(string password)
        {
            return BCryptNet.Verify(password, Password);
        }

        public void UpdateFromAnotherAccount(Account acc)
        {
            this.Username = acc.Username;
            this.Email = acc.Email;
            this.Phone = acc.Phone;
            this.Mobile = acc.Mobile;
            this.ShippingDetails = acc.ShippingDetails;
            this.CustomerId = acc.CustomerId;
            if (this.Role >= AccountRole.Admin)
            {
                this.Role = acc.Role;
            }

            this.UpdatedAt = DateTime.UtcNow;

        }
    }
}
