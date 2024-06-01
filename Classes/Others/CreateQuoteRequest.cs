using prod_server.Entities;

namespace prod_server.Classes.Others {
    public class CreateQuoteRequest {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? EmailAddress { get; set; }
        public string? ContactName { get; set; }
        public string? PhoneNumber { get; set; }
        public TypeOfBusiness TypeOfBusiness { get; set; } = TypeOfBusiness.Other;
        public virtual List<CartProduct> Products { get; set; } = new List<CartProduct>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}