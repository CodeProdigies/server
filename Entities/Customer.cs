using prod_server.Classes.Others;
using System.ComponentModel.DataAnnotations;

namespace prod_server.Entities
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Country { get; set; }
        public string? Identifier { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set;  }
        public virtual List<Order> Order { get; set; }
        public virtual List<Quote> Quote { get; set; }
        public virtual List<Account> Account { get; set; }

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
            Address = modal.Address;
            City = modal.City;
            State = modal.State;
            Zip = modal.Zip;
            Country = modal.Country;
            Identifier = modal.Identifier;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

        }

    }
}
