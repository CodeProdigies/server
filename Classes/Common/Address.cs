namespace prod_server.Classes.Common
{
    public class Address
    {
        public string Country { get; set; } = "";
        public string ShippingAddress { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string ZipCode { get; set; } = "";

        public Address(string country = "", string shippingAddress = "", string city = "", string state = "", string zipCode = "")
        {
            Country = country;
            ShippingAddress = shippingAddress;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        // Method to check if the address is fully populated
        public bool IsComplete()
        {
            return !string.IsNullOrEmpty(Country) &&
                   !string.IsNullOrEmpty(ShippingAddress) &&
                   !string.IsNullOrEmpty(City) &&
                   !string.IsNullOrEmpty(State) &&
                   !string.IsNullOrEmpty(ZipCode);
        }

        // Method to format the address as a single string
        public string FormatAddress()
        {
            return $"{ShippingAddress}, {City}, {State}, {ZipCode}, {Country}";
        }

        // Method to update the address properties
        public void UpdateAddress(Address newAddress)
        {
            Country = newAddress.Country ?? Country;
            ShippingAddress = newAddress.ShippingAddress ?? ShippingAddress;
            City = newAddress.City ?? City;
            State = newAddress.State ?? State;
            ZipCode = newAddress.ZipCode ?? ZipCode;
        }

        // Method to validate the address format (basic example)
        public bool Validate()
        {
            // Example: check if country and shippingAddress are not empty
            return !string.IsNullOrEmpty(Country) && !string.IsNullOrEmpty(ShippingAddress);
        }

        // Method to clear the address properties
        public void Clear()
        {
            Country = string.Empty;
            ShippingAddress = string.Empty;
            City = string.Empty;
            State = string.Empty;
            ZipCode = string.Empty;
        }

        // Method to compare two addresses
        public bool IsEqual(Address other)
        {
            return Country == other.Country &&
                   ShippingAddress == other.ShippingAddress &&
                   City == other.City &&
                   State == other.State &&
                   ZipCode == other.ZipCode;
        }

        // Method to format the address as an object
        public Dictionary<string, string?> ToObject()
        {
            return new Dictionary<string, string?>
        {
            { "Country", Country },
            { "ShippingAddress", ShippingAddress },
            { "City", City },
            { "State", State },
            { "ZipCode", ZipCode }
        };
        }

        // Getter for the full address
        public string FullAddress
        {
            get
            {
                return $"{ShippingAddress}, {City}, {State} {ZipCode}, {Country}";
            }
        }

        // Getter for city and state
        public string CityState
        {
            get
            {
                return $"{City}, {State}";
            }
        }

        // Getter for a country-specific formatted address
        public string FormattedAddress
        {
            get
            {
                if (Country.ToLower() == "usa" || Country.ToLower() == "united states")
                {
                    return $"{ShippingAddress}, {City}, {State} {ZipCode}";
                }
                else
                {
                    return $"{ShippingAddress}, {City}, {ZipCode}, {State}, {Country}";
                }
            }
        }
    }
}
