namespace Book.Application.Domain
{
    public class Paged<T> where T : class
    {
        public Paged(IEnumerable<T> data, int totalSize, int currentpage, int pageSize)
        {
            Records = data;
            TotalSize = totalSize;
            CurrentPage = currentpage;
            PageSize = pageSize;
        }
        public int TotalSize { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public IEnumerable<T> Records { get; private set; }
    }
}
