namespace prod_server.Classes.Others
{
    public class GenericSearchFilter
    {
        public GenericSearchFilter() {}
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; } = "asc";
        public Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>();
        public List<string> Includes { get; set; } = new List<string>();
        public void Add(string key, string value)
        {
            Filters.TryAdd(key, value);
        }
    }
}
