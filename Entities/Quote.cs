using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod_server.Entities
{
    [Table("Quotes")]
    public class Quote
    {
        [Key]
        [Column("id")]
        public Guid? Id { get; set; } = Guid.NewGuid();
        [Column("name")]
        public string? Name { get; set; }
        [Column("descrption")]
        public string? Description { get; set; }
        [Column("email")]
        public string? EmailAddress { get; set; }
        [Column("contact_name")]
        public string? ContactName { get; set; }
        [Column("phone_number")]
        public string? PhoneNumber { get; set; }
        [Column("type_of_business")]
        public TypeOfBusiness TypeOfBusiness { get; set; } = TypeOfBusiness.Other;
        [Column("products")]
        public virtual List<CartProduct> Products { get; set; }
        [Column("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}


public enum TypeOfBusiness
{
    Dentist,
    SurgeryCenter,
    Hospital,
    Veterinarian,
    Restaurant,
    Construction,
    Other,
}