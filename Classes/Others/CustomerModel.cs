using prod_server.Classes.Common;

namespace prod_server.Classes.Others
{
    public class CustomerModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public Address ShippingDetails { get; set; }
        public string? Identifier { get; set; }
    }
}
