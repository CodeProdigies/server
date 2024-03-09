using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod_server.Entities
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = new Guid();

        [Required]
        [Column("sku")]
        public string SKU { get; set; } // "Stock Keeping Unit
        [Required]
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("price")]
        public decimal Price { get; set; }
        [Column("image")]
        public string? Image { get; set; }
        [Column("category")]
        public ProductsCategory? Category { get; set; }
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
