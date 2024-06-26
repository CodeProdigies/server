﻿using prod_server.Classes;
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
        public virtual List<UploadedFile> Files { get; set; } = [];
        [Column("category")]
        public ProductsCategory? Category { get; set; }
        [NotMapped]
        public virtual List<CartProduct>? CartProducts { get; set; }
        [NotMapped]
        public virtual List<OrderItem>? OrderItems{ get; set; }
        public virtual Provider? Provider { get; set; }
        public int? ProviderId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        [Column("is_archived")]
        public bool isArchived { get; set; } = false;

        public Product() { }

        public override string ToString()
        {
            return $"Product: {Name} - {Description} - {Price} - {Category}";
        }
    }

    public enum ProductsCategory
    {
        [Description("Category One")]
        CategoryOne,
    }
}
