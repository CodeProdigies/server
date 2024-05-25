namespace prod_server.Classes.Others
{
    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public string NextPage { get; set; }
        public string PreviousPage { get; set; }
        public string FirstPage { get; set; }
        public string LastPage { get; set; }
        public int PageCount { get; set; } = 0;
        public List<T> Data { get; set; } = new List<T>();

        public PagedResult(List<T> data, int page, int pageSize, int total)
        {
            Page = page;
            PageSize = pageSize;
            Total = total;
            TotalPages = (int)Math.Ceiling((double)total / pageSize);
            HasNext = Page < TotalPages;
            HasPrevious = Page > 1;
            NextPage = HasNext ? $"?page={Page + 1}&pageSize={PageSize}" : null;
            PreviousPage = HasPrevious ? $"?page={Page - 1}&pageSize={PageSize}" : null;
            FirstPage = $"?page=1&pageSize={PageSize}";
            LastPage = $"?page={TotalPages}&pageSize={PageSize}";
            PageCount = (int)Math.Ceiling((double)total / pageSize);
            Data = new List<T>(data);
        }
    }
}
