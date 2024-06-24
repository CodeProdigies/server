namespace prod_server.Classes.Common
{
    public class Name
    {
        public string First { get; set; } = "";
        public string? Middle { get; set; }
        public string Last { get; set; } = "";
        public string? Title { get; set; }
        public string? Suffix { get; set; }

        public override string ToString()
        {
            return GetFormattedName("lastFirst");
        }

        public string GetInitials()
        {
            var initials = First.Length > 0 ? char.ToUpper(First[0]).ToString() : string.Empty;
            if (!string.IsNullOrEmpty(Middle))
            {
                initials += char.ToUpper(Middle[0]);
            }
            if (Last.Length > 0)
            {
                initials += char.ToUpper(Last[0]);
            }
            return initials;
        }

        public string GetFullName()
        {
            var fullName = First;
            if (!string.IsNullOrEmpty(Middle))
            {
                fullName += $" {Middle}";
            }
            fullName += $" {Last}";
            return fullName;
        }

        public string GetFormattedName(string format = "firstLast")
        {
            return format switch
            {
                "firstLast" => $"{First} {Last}",
                "lastFirst" => $"{Last}, {First}",
                "last" => Last,
                "first" => First,
                _ => string.Empty,
            };
        }

        public bool ValidateName()
        {
            return !string.IsNullOrWhiteSpace(First) && !string.IsNullOrWhiteSpace(Last);
        }
    }
}
