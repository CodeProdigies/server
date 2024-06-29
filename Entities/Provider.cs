using prod_server.Classes.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod_server.Entities
{
    [Table("Providers")]
    public class Provider
    {
        [Column("id")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Address ShippingDetails { get; set; } = new Address();
        public string Website { get; set; }
        public string Identifier { get; set; }
        public virtual List<Product> Products { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool isArchived { get; set; } = false;
    }
}
