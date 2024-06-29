using prod_server.Classes;
using prod_server.Classes.Others;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod_server.Entities
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [Column("id")]
        public int? Id { get; set; }
        public virtual List<OrderItem> Products { get; set; } = new List<OrderItem>();
        [Column("total")]
        public decimal Total { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        [Column("is_archived")]
        public bool isArchived { get; set; } = false;

        
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

        public Order (CreateQuoteRequest createQuoteRequest){
            var listOfProductsFromQuote = createQuoteRequest.Products
                .Where(x => x.Product != null)    
                .Select(cartItem => new OrderItem(cartItem.Product!)
                {
                    Quantity = cartItem.Quantity,
                    SellPrice = cartItem.Product?.Price ?? 0,
                    BuyPrice = 0,
                    Total = (cartItem.Product?.Price ?? 0) * cartItem.Quantity
                }).ToList();

            Products = listOfProductsFromQuote;
            Products.ForEach(product => Total += product.Total);
            Total = Products.Sum(x => x.Total);
            CreatedAt = DateTime.UtcNow;
        
        }

        public enum OrderStatus
        {
            Pending = 10,
            Processing = 20,
            Shipped = 30,
            Delivered = 40,
            Cancelled = 50
        }

    }
}
