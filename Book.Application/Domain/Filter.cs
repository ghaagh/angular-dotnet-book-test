namespace Book.Application.Domain
{
    public class Filter
    {
        public string OrderBy { get; set; }
        public string SearchValue { get; set; }
        public string SearchFields { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
