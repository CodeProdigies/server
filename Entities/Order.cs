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
        public virtual List<OrderItem> Products { get; set; } = new List<OrderItem>();
        [Column("total")]
        public decimal Total { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        
        public Order()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public Order (Quote quote)
        {

            var listOfProductsFromQuote = quote.Products
                .Where(x => x.Product != null)    
                .Select(cartItem =>
                {
                    var orderItem = new OrderItem(cartItem.Product!);
                    orderItem.Quantity = cartItem.Quantity;
                    orderItem.SellPrice = cartItem.Product.Price;
                    orderItem.BuyPrice = 0;
                    orderItem.Total = cartItem.Product.Price * cartItem.Quantity;
                    return orderItem;
                }).ToList();
            

            Products = listOfProductsFromQuote;
            Products.ForEach(product => Total += product.Total);
            CreatedAt = DateTime.UtcNow;
        }

    }
}
