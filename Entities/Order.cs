using prod_server.Classes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod_server.Entities
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public List<OrderItem> Products { get; set; } = new List<OrderItem>();
        [Column("total")]
        public decimal Total { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

    }
}
