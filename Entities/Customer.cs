using prod_server.Classes.Common;
using prod_server.Classes.Others;
using System.ComponentModel.DataAnnotations;

namespace prod_server.Entities
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Phone { get; set; }
        public Address ShippingDetails { get; set; } = new Address();
        public string? Identifier { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual List<Order> Orders { get; set; } = [];
        public virtual List<Quote> Quotes { get; set; } = [];
        public virtual List<Account> Accounts { get; set; } = [];
        public virtual List<UploadedFile> Files { get; set; } = [];

        public Customer()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public Customer(CustomerModel modal)
        {
            Name = modal.Name;
            Email = modal.Email;
            Phone = modal.Phone;
            ShippingDetails = modal.ShippingDetails;
            Identifier = modal.Identifier;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

        }

    }
}
