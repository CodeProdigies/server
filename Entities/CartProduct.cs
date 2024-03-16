using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace prod_server.Entities
{
    public class CartProduct
    {
        [Column("Id")]
        public Guid? Id { get; set; }
        [ForeignKey("Product")]
        [Column("ProductId")]
        public Guid? ProductId { get; set; }
        [ForeignKey("Quote")]
        [Column("QuoteId")]
        public Guid? QuoteId { get; set; }

        [JsonIgnore]
        [NotMapped]
        public virtual Quote? Quote{ get; set; }
        
        public virtual Product? Product { get; set; }

        [Column("Quantity")]
        public int Quantity { get; set; }
    }
}