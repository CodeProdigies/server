using prod_server.Classes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace prod_server.Entities
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [Column("id")]
        public Guid? Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("sku")]
        public string SKU { get; set; } // "Stock Keeping Unit
        [Required]
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("cost")]
        public decimal? Cost { get; set; } = 0;
        [Column("price")]
        public decimal Price { get; set; }
        [AllowNull]
        [Column("image")]
        public string? Image { get; set; }
        [Column("category")]
        public ProductsCategory? Category { get; set; }
        [NotMapped]
        public virtual List<CartProduct>? CartProducts { get; set; }
        [NotMapped]
        public virtual List<OrderItem>? OrderItems{ get; set; }
        public Product() { }

        public override string ToString()
        {
            return $"Product: {Name} - {Description} - {Price} - {Image} - {Category}";
        }
    }

    public enum ProductsCategory
    {
        [Description("Category One")]
        CategoryOne,
    }
}
