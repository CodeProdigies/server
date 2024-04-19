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
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
        public string Identifier { get; set; }
        public virtual List<Product> Products { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
