using prod_server.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod_server.Classes
{
    public class OrderItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Product")]
        [Column("product_id")]
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("is_sold")]
        public bool IsSold { get; set; }

        [Column("total")]
        public decimal Total { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [NotMapped]
        public virtual Order? Order { get; set; }
    }
}
