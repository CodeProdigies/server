using iText.Kernel.Geom;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace prod_server.Classes.Others
{
    public class PagedResult<T>
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public int Total { get; set; } = 0;
        public int TotalPages { get; set; }
        public bool HasNext { get; set; } = false;
        public bool HasPrevious { get; set; } = false;
        public string NextPage { get; set; } = null;
        public string PreviousPage { get; set; } = null;
        public string FirstPage { get; set; } = "?page=1&pageSize=10";
        public string LastPage { get; set; } = "?page=1&pageSize=10";
        public int PageCount { get; set; } = 0;
        public List<T> Data { get; set; } = new List<T>();

        public PagedResult()
        {

        }
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
